﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace translit.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.6.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool PronunciationActivated {
            get {
                return ((bool)(this["PronunciationActivated"]));
            }
            set {
                this["PronunciationActivated"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Chinese2EnglishEnabled {
            get {
                return ((bool)(this["Chinese2EnglishEnabled"]));
            }
            set {
                this["Chinese2EnglishEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("US")]
        public string PronunciationAccent {
            get {
                return ((string)(this["PronunciationAccent"]));
            }
            set {
                this["PronunciationAccent"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection userID {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["userID"]));
            }
            set {
                this["userID"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8511813095152")]
        public string SogouSecret {
            get {
                return ((string)(this["SogouSecret"]));
            }
            set {
                this["SogouSecret"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"SUID=55D8FB72AF67900A000000005BBB6A92; SELECTION_SWITCH=1; HISTORY_SWITCH=1; SMYUV=1545743088257814; wuid=AAGwa3OKJgAAAAqLFD02RQYAGwY=; CXID=0F46B9B8E19611E22755D23CC9098366; ssuid=6745682256; sct=23; LSTMV=410%2C22; LCLKINT=13443; ad=zkllllllll2NWXH5lllllVCfW1DlllllbRTwGkllll9lllllRllll5@@@@@@@@@@; ABTEST=0|1570572735|v17; SUV=1570572709885; SNUID=B9684BC2B0AA3B3C68816FD8B07AF98D; IPLOC=CN1100")]
        public string SogouCookie {
            get {
                return ((string)(this["SogouCookie"]));
            }
            set {
                this["SogouCookie"] = value;
            }
        }
    }
}
