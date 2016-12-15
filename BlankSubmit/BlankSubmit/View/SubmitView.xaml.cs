using Xamarin.Forms;
using XLabs.Forms.Validation;
using Validator = BlankSubmit.Helpers.Validation.Validator;

namespace BlankSubmit.View
{
    public partial class SubmitView : ContentPage
    {
        public Validator Validator;

        public SubmitView()
        {
            InitializeComponent();
        }
    }
}
