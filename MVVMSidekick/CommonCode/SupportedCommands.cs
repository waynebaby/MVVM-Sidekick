using System;
using System.Collections.Generic;
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

				var elementFrag = XDocument.Load(f1)
				 .Descendants().Single(x => x.Name.LocalName == "packages");
				var framework = elementFrag.Elements().First().Attribute("targetFramework").Value;

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

				var vsixPath = new FileInfo(extensionFile).Directory.GetFiles("*.csproj",  SearchOption.AllDirectories).Single().FullName;

				var dvisx = XDocument.Load(vsixPath);
				var itemGroup = dvisx.Descendants()
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
				var newNd = new XElement("Content",
					  new XAttribute("Include", Path.Combine(Environment.CurrentDirectory,"packages" ,string.Format ("{0}\\{0}")))
					);


				//foreach (var item in packages)
				//{
				//	Console.WriteLine(item);
				//}



				Console.Read();
			},
			OnHelp = () =>
			{
				Console.WriteLine("commoncode.exe DPEXT <XML File Name> <Extension project path>");
			}


		};
		#endregion

	}
}