﻿#pragma checksum "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "C747A22CDD10B24ACB4998E17B6982249B415CD2798D1EC9ADE2ECC7094FD2E3"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TelegramBot.Pages.AdminPanel;


namespace TelegramBot.Pages.AdminPanel {
    
    
    /// <summary>
    /// PageVoiceSettings
    /// </summary>
    public partial class PageVoiceSettings : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 30 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CmbBoxAllVoices;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Run RnVoiceName;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Run RnVoiceCulture;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CmbBoxSavedVoices;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Run RnSelectedVoiceName;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Run RnSelectedVoiceCulture;
        
        #line default
        #line hidden
        
        
        #line 126 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxbVoiceCost;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TelegramBot;component/pages/adminpanel/pagevoicesettings.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.CmbBoxAllVoices = ((System.Windows.Controls.ComboBox)(target));
            
            #line 31 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
            this.CmbBoxAllVoices.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CmbBoxChangedAddVoice);
            
            #line default
            #line hidden
            return;
            case 2:
            this.RnVoiceName = ((System.Windows.Documents.Run)(target));
            return;
            case 3:
            this.RnVoiceCulture = ((System.Windows.Documents.Run)(target));
            return;
            case 4:
            
            #line 57 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnClickAddVoice);
            
            #line default
            #line hidden
            return;
            case 5:
            this.CmbBoxSavedVoices = ((System.Windows.Controls.ComboBox)(target));
            
            #line 88 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
            this.CmbBoxSavedVoices.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CmbBoxChangedChangeVoice);
            
            #line default
            #line hidden
            return;
            case 6:
            this.RnSelectedVoiceName = ((System.Windows.Documents.Run)(target));
            return;
            case 7:
            this.RnSelectedVoiceCulture = ((System.Windows.Documents.Run)(target));
            return;
            case 8:
            this.TxbVoiceCost = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            
            #line 139 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnClickDeleteVoice);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 159 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnClickSaveChanges);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 178 "..\..\..\..\Pages\AdminPanel\PageVoiceSettings.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnClickGoBack);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

