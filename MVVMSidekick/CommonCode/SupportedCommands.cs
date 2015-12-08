using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CommonCode
{

	public static class Commands
	{
		static Commands()
		{
			var items =
			  typeof(Commands)
			  .GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
			  .Where(x => x.FieldType == typeof(ICommandLineCommand))
			  .Select(x => x.GetValue(null) as ICommandLineCommand)
			  .ToDictionary(x => x.CommandKeyword);

			dics = new SortedDictionary<string, ICommandLineCommand>(
				items,
				Comparer<string>.Create(
					(x, y) =>
					string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase)));

		}

		static SortedDictionary<string, ICommandLineCommand>
		   dics;

		public static ICommandLineCommand GetCommand(string commandName)
		{
			return dics[commandName];
		}

		/// <summary>
		/// 将所有的工程 中package.config 文件里的依赖包信息 放入 nuget spec 索引中，保证依赖
		/// </summary>
		public static readonly ICommandLineCommand DPGRP
		#region DPGRP
		= new CommandLineCommand(nameof(DPGRP), null)
		{

			OnExecute = args =>
			{
				if (args.Length < 3)
				{
					throw new IndexOutOfRangeException("Need at least 2 file path: from, to");

				}
				var f1 = args[1];
				var f2 = args[2];

				if (!File.Exists(f1))
				{
					throw new ArgumentException("from file not exists");
				}

				if (!File.Exists(f2))
				{
					throw new ArgumentException("to file not exists");
				}

				XElement elementFrag = null;
				string framework = null;

				if (f1.EndsWith(".config"))
				{
					elementFrag = XDocument.Load(f1)
						.Descendants().Single(x => x.Name.LocalName == "packages");

					framework = elementFrag.Elements().First().Attribute("targetFramework").Value;
				}
				else
				{

					framework = "uap10.0";
					var obj = JObject.Load(new JsonTextReader(new StreamReader(f1)));

					var deps = obj["dependencies"];



					elementFrag = new XElement("packages");



					foreach (JProperty item in deps)
					{
						elementFrag.Add(new XElement("package",
							new XAttribute("version", item.Value),
							new XAttribute("id", item.Name)
							));
						//Console.WriteLine(item.Name );
					}
				}

				var docnusp = XDocument.Load(f2);
				var dependencies = docnusp
				.Descendants().Single(x => x.Name.LocalName == "dependencies");

				var ns = dependencies.Name.NamespaceName;
				var gp = dependencies.Elements()
					  .Where(g => g.Name.LocalName == "group")
					  .Where(g => g.Attributes()
						 .Where(x => x.Name == "targetFramework")
						 .Single()
						 .Value == framework)
					  .FirstOrDefault();
				if (gp == null)
				{
					gp = new XElement(XName.Get("group", ns), new XAttribute("targetFramework", framework));
					//if (framework != "uap10.0")
					//{
					//		 gp.Add ();
					//}
					dependencies.Add(gp);
				}
				else
				{
					gp.RemoveNodes();
				}

				foreach (var ele in elementFrag.Elements().Where(x => x.Name.LocalName == "package"))
				{
					ele.Name = XName.Get("dependency", ns);
					var remover = ele.Attributes().Where(
						x => x.Name == "targetFramework"
						);
					foreach (var item in remover)
					{
						Console.WriteLine(item.Name);
						item.Remove();
					}
					remover = ele.Attributes().Where(
						x => x.Name == "userInstalled"
						);
					foreach (var item in remover)
					{
						Console.WriteLine(item.Name);
						item.Remove();
					}


					var versions = ele.Attributes().Where(
						x => x.Name == "version");
					foreach (var version in versions)
					{

						if (version.Value.Trim(new[] { '(', ')', '[', ']' }) == version.Value)
						{
							version.Value = string.Format("[{0},{1})", version.Value, "100.0");
						}
					}


					gp.Add(ele);
				}

				docnusp.Save(f2);

			}


		};
		#endregion



		/// <summary>
		/// 更新扩展工程中的文件引用
		/// </summary>
		public static readonly ICommandLineCommand DPEXT
		#region DPEXT
		= new CommandLineCommand(nameof(DPEXT), null)
		{
			OnExecute = args =>
			{
				var xmlDoc = XDocument.Load(args[1]);
				var packages = xmlDoc.Descendants(XName.Get("project")).Select(x => x.Value)
					.SelectMany(p =>
					   new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, p)).GetFiles("packages.config"))
					.Select(p =>
					   XDocument.Load(p.FullName))
					.SelectMany(xd =>
					   xd.Descendants(XName.Get("package")).Select(nd => nd.Attribute(XName.Get("id")).Value + "." + nd.Attribute(XName.Get("version")).Value))
					.Distinct();
				var extensionPath = args[2];
				var extensionFile = Directory.GetFiles(extensionPath, "*.vsixmanifest", SearchOption.AllDirectories).FirstOrDefault();
				if (extensionFile == null)
				{
					throw new FileNotFoundException();
				}

				var vsixPath = new FileInfo(extensionFile).Directory.GetFiles("*.csproj", SearchOption.AllDirectories).Single().FullName;

				var dvsix = XDocument.Load(vsixPath);
				var ns = dvsix.Root.Name.Namespace;
				var itemGroup = dvsix.Descendants()
					.Where(x => x.Name.LocalName == "None" && x.Attributes().Any(a => a.Name.LocalName == "Include" && a.Value.EndsWith(".vsixmanifest")))
					.Select(x => x.Parent)
					.Single();

				var incs = itemGroup.Elements().Where(x => x.Name.LocalName == "Content" && x.Attributes().Any(a => a.Name.LocalName == "Include" && a.Value.EndsWith(".nupkg")));
				incs.ToList().ForEach(x => x.Remove());
				/*    
				<Content Include="..\..\packages\Microsoft.Bcl.1.1.9\Microsoft.Bcl.1.1.9.nupkg">
				  <Link>Packages\Microsoft.Bcl.1.1.9.nupkg</Link>
				  <IncludeInVSIX>true</IncludeInVSIX>
				</Content>*/

				foreach (var package in packages)
				{
					var p = string.Format("{0}\\{0}.nupkg", package);
					if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "packages", p)))
					{
						p = string.Format("{0}\\{0}.nupkg", package.Remove(package.Length - 2));
					}
					if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "packages", p)))
					{
						p = string.Format("{0}.0\\{0}.0.nupkg", package);

					}
					var newNd = new XElement(XName.Get("Content", ns.NamespaceName),
							new XAttribute("Include", Path.Combine(Environment.CurrentDirectory, "packages", p)),
							new XElement(XName.Get("Link", ns.NamespaceName), string.Format("Packages\\{0}.nupkg", package)),
							new XElement(XName.Get("IncludeInVSIX", ns.NamespaceName), "true")
						  );
					itemGroup.Add(newNd);
				}


				var currentPackageVersion = XDocument.Load(@"CommonCode\CurrentPackageVersion.xml").Descendants().First(x => x.Name.LocalName == "id").Value;
				var newMSKNd = new XElement(XName.Get("Content", ns.NamespaceName),
							new XAttribute("Include", Path.Combine(Environment.CurrentDirectory, "Nuget", string.Format("MVVM-Sidekick.{0}.nupkg", currentPackageVersion))),
							new XElement(XName.Get("Link", ns.NamespaceName), string.Format("Packages\\MVVM-Sidekick.{0}.nupkg", currentPackageVersion)),
							new XElement(XName.Get("IncludeInVSIX", ns.NamespaceName), "true")
						  );
				itemGroup.Add(newMSKNd);


				Console.Write(dvsix);
				dvsix.Save(vsixPath);

				//Console.Read();
			},
			OnHelp = () =>
			{
				Console.WriteLine("commoncode.exe DPEXT <XML File Name> <Extension project path>");
			}


		};
		#endregion


		/// <summary>
		/// 更新nuget spec中的版本号
		/// </summary>
		public static readonly ICommandLineCommand UPVER
		#region UPVER
		= new CommandLineCommand(nameof(UPVER), null)
		{
			OnExecute = args =>
			  {
				  if (args.Length < 2)
				  {
					  throw new IndexOutOfRangeException("need path of nuget spec file");
				  }

				  if (!File.Exists(args[1]))
				  {
					  throw new IndexOutOfRangeException("nuget spec file not exists");
				  }

				  var d = XDocument.Load(args[1]);
				  var packages = XDocument.Load(@"CommonCode\CurrentPackageVersion.xml").Root
							  .Elements()
							  .Where(x => x.Name.LocalName == "version");
				  var rnotes = d.Descendants().First(x => x.Name.LocalName == "releaseNotes");
				  rnotes.Value = "";
				  foreach (var currentPackage in packages)
				  {
					  var currentPackageVersion = currentPackage
						.Descendants()
						.Where(x => x.Name.LocalName == "id" && x.Parent.Name.LocalName == "version")
						.First()
						.Value;



					  var currentPackageReleaseNotes = currentPackage
						.Descendants()
						.Where(x => x.Name.LocalName == "releaseNotes" && x.Parent.Name.LocalName == "version")
						.First()
						.Value;
					  rnotes.Value = rnotes.Value + string.Format("\r\n\t{0}\r\n\t\t{1}\r\n", currentPackageVersion, currentPackageReleaseNotes);
				  }



				  var ver = d.Descendants().First(x => x.Name.LocalName == "version");
				  ver.Value = packages.First()
						.Descendants()
						.Where(x => x.Name.LocalName == "id" && x.Parent.Name.LocalName == "version")
						.First()
						.Value; ;
				  d.Save(args[1]);
			  }
		};
		#endregion


		/// <summary>
		/// 更新每个模板文件中的包版本
		/// </summary>
		public static readonly ICommandLineCommand DPTML
		#region DPTML
		= new CommandLineCommand(nameof(DPTML), null)
		{
			OnExecute = args =>
			{

				var templateRootDir = args[1];
				var templateDefFiles = Directory.GetFiles(templateRootDir, "*.vstemplate", SearchOption.AllDirectories);
				var extensionFile = Directory.GetFiles(templateRootDir, "*.vsixmanifest", SearchOption.AllDirectories).FirstOrDefault();
				if (extensionFile == null)
				{
					throw new FileNotFoundException();
				}

				var vsixPath = new FileInfo(extensionFile).Directory.GetFiles("*.csproj", SearchOption.AllDirectories).Single().FullName;


				var dvsix = XDocument.Load(vsixPath);
				var ns = dvsix.Root.Name.Namespace;
				var itemGroup = dvsix.Descendants()
					.Where(x => x.Name.LocalName == "None" && x.Attributes().Any(a => a.Name.LocalName == "Include" && a.Value.EndsWith(".vsixmanifest")))
					.Select(x => x.Parent)
					.Single();

				var incs = itemGroup.Elements().Where(x => x.Name.LocalName == "Content" && x.Attributes().Any(a => a.Name.LocalName == "Include" && a.Value.EndsWith(".nupkg")));

				/*    
				<Content Include="..\..\packages\Microsoft.Bcl.1.1.9\Microsoft.Bcl.1.1.9.nupkg">
				  <Link>Packages\Microsoft.Bcl.1.1.9.nupkg</Link>
				  <IncludeInVSIX>true</IncludeInVSIX>
				</Content>*/

				var pksInVSIX = incs.SelectMany(x => x.Attributes().Where(a => a.Name.LocalName == "Include").Select(a => a.Value))
					.Select(x => Path.GetFileNameWithoutExtension(x));
				var pksInVSIXSet = new SortedSet<string>(pksInVSIX);

				/*    <packages repository="extension" repositoryId="MVVM_Sidekick_Extensions.waywa msft.d1fabcfa-5ffc-4756-b047-5cfbd2931a24">
							<package id="Rx-Core" version="2.2.5"  />
							<package id="Rx-Interfaces" version="2.2.5"  />
							<package id="Rx-Linq" version="2.2.5"  />
							<package id="Rx-Main" version="2.2.5"  />
							<package id="Rx-PlatformServices" version="2.2.5"  />
							<package id="Rx-Xaml" version="2.2.5"  />
							<package id="MVVM-Sidekick"  version="1.4.20150605.16300000"/>*/

				foreach (var docp in templateDefFiles.Select(f => new { doc = XDocument.Load(f), path = f }))
				{
					var nodes =
						docp.doc
								.Descendants()
								.Where(
									x =>
										x.Name.LocalName == "packages"
										&& x.Attributes().Any(a => a.Name.LocalName == "repository" && a.Value == "extension")
										&& x.Attributes().Any(a => a.Name.LocalName == "repositoryId" && a.Value.Contains("MVVM") && a.Value.Contains("Sidekick")));

					var packages = nodes.SelectMany(x => x.Elements().Where(p => p.Name.LocalName == "package"));
					foreach (var package in packages)
					{
						var ida = package.Attributes().Single(x => x.Name.LocalName == "id");
						var versiona = package.Attributes().Single(x => x.Name.LocalName == "version");
						var tryPack = string.Format("{0}.{1}", ida.Value, versiona.Value);
						if (!pksInVSIXSet.Contains(tryPack))
						{
							var n = (char)('.' + 1);
							var item = pksInVSIXSet.GetViewBetween(ida.Value, ida.Value + n).First();
							versiona.Value = item.Remove(0, ida.Value.Length + 1);

						}
						Console.WriteLine(package);
					}

					docp.doc.Save(docp.path);
				}






			},
			OnHelp = () =>
			{
				Console.WriteLine("commoncode.exe DPEXT <XML File Name> <Extension project path>");
			}


		};
		#endregion


	}
}