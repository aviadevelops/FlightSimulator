﻿#pragma checksum "..\..\..\PlayControllerView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B5C41FE42597365D5211317AC04EAF0E1E63B70A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FlightSimulator;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace FlightSimulator {
    
    
    /// <summary>
    /// PlaybackScreen
    /// </summary>
    public partial class PlaybackScreen : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\..\PlayControllerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FlightSimulator.PlaybackScreen window_flighgear;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\PlayControllerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_open;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\PlayControllerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_back;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\PlayControllerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_play;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\PlayControllerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_foward;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\PlayControllerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_pause;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\PlayControllerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider slider_timesteps;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\PlayControllerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label_display_time;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\PlayControllerView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtbox_change_speed;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.4.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FlightSimulator;V1.0.0.0;component/playcontrollerview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\PlayControllerView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.4.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.window_flighgear = ((FlightSimulator.PlaybackScreen)(target));
            
            #line 8 "..\..\..\PlayControllerView.xaml"
            this.window_flighgear.Closing += new System.ComponentModel.CancelEventHandler(this.window_flightgear_closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btn_open = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\..\PlayControllerView.xaml"
            this.btn_open.Click += new System.Windows.RoutedEventHandler(this.btn_open_click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btn_back = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\PlayControllerView.xaml"
            this.btn_back.Click += new System.Windows.RoutedEventHandler(this.btn_back_click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btn_play = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\PlayControllerView.xaml"
            this.btn_play.Click += new System.Windows.RoutedEventHandler(this.btn_play_click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn_foward = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\PlayControllerView.xaml"
            this.btn_foward.Click += new System.Windows.RoutedEventHandler(this.btn_foward_click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btn_pause = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\PlayControllerView.xaml"
            this.btn_pause.Click += new System.Windows.RoutedEventHandler(this.btn_pause_click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.slider_timesteps = ((System.Windows.Controls.Slider)(target));
            
            #line 24 "..\..\..\PlayControllerView.xaml"
            this.slider_timesteps.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.Slider_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.label_display_time = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.txtbox_change_speed = ((System.Windows.Controls.TextBox)(target));
            
            #line 26 "..\..\..\PlayControllerView.xaml"
            this.txtbox_change_speed.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtbox_change_speed_TextChanged);
            
            #line default
            #line hidden
            
            #line 26 "..\..\..\PlayControllerView.xaml"
            this.txtbox_change_speed.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtbox_change_speed_pressed);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

