//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("BrainPod.Registration.xaml", "Registration.xaml", typeof(global::BrainPod.Registration))]

namespace BrainPod {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Registration.xaml")]
    public partial class Registration : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::BrainPod.CustomEntry FirstNameInput;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::BrainPod.CustomEntry SecondNameInput;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::BrainPod.CustomEntry EmailInput;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::BrainPod.CustomEntry PasswordInput;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(Registration));
            FirstNameInput = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::BrainPod.CustomEntry>(this, "FirstNameInput");
            SecondNameInput = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::BrainPod.CustomEntry>(this, "SecondNameInput");
            EmailInput = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::BrainPod.CustomEntry>(this, "EmailInput");
            PasswordInput = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::BrainPod.CustomEntry>(this, "PasswordInput");
        }
    }
}