using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Components
{
    public class MVVMSidekickValidator : ComponentBase
    {
        [CascadingParameter]
        private EditContext CurrentEditContext { get; set; }


        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException("MVVMSidekickValidator requires a cascading parameter of type EditContext. For example, you can use MVVMSidekickValidator inside an EditForm.");
            }
            var model = CurrentEditContext.Model as BindableBase;
            if (model == null)
            {
                throw new InvalidOperationException("MVVMSidekickValidator requires model is a BindableBase.");
            }
            var errorModel = CurrentEditContext.Model as INotifyDataErrorInfo;
            if (errorModel == null)
            {
                throw new InvalidOperationException("MVVMSidekickValidator requires model of the form implements INotifyDataErrorInfo.");
            }
            ValidationMessageStore messages = new ValidationMessageStore(CurrentEditContext);
            errorModel.ErrorsChanged += (o, e) =>
            {

                messages.Clear();
                foreach (var errorEntry in model.GetAllErrors())
                {
                    FieldIdentifier fieldIdentifier = CurrentEditContext.Field(errorEntry.PropertyName);
                    messages.Add(in fieldIdentifier, errorEntry.Message);
                }

            };

            CurrentEditContext.OnValidationRequested += (o, e) =>
              {
                  messages.Clear();
                  foreach (var errorEntry in model.GetAllErrors())
                  {
                      FieldIdentifier fieldIdentifier = CurrentEditContext.Field(errorEntry.PropertyName);
                      messages.Add(in fieldIdentifier, errorEntry.Message);
                  }
              };

        }


    }
}
