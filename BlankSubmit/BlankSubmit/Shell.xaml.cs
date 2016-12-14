using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XLabs.Forms.Controls;
using XLabs.Forms.Services;

namespace BlankSubmit
{
    public partial class Shell : NavigationPage
    {
        public static NavigationService NavigationService { get; private set; }

        public Shell()
        {    
            var a = new AutoCompleteView();
            InitializeComponent();
            NavigationService = new NavigationService(this.Navigation);
            NavigationService.NavigateTo(typeof(SubmitView));
        }
    }
}
