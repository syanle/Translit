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
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("pdfTranslater.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://translate.yandex.net/api/v1.5/tr.json/getLangs?ui=en&amp;key=trnsl.1.1.20180310T213839Z.5346cde1e4314c12.5f0210740a735e6e86d15e0f521b17a48d81edb9.
        /// </summary>
        internal static string getLangsUrl {
            get {
                return ResourceManager.GetString("getLangsUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to translit!.
        /// </summary>
        internal static string Secret {
            get {
                return ResourceManager.GetString("Secret", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://fanyi.sogou.com/reventondc/translate.
        /// </summary>
        internal static string SogouTranslaterUrl {
            get {
                return ResourceManager.GetString("SogouTranslaterUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://www.translit.tk.
        /// </summary>
        internal static string TranslitAboutUrl {
            get {
                return ResourceManager.GetString("TranslitAboutUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://www.translit.tk/update/check?userid=.
        /// </summary>
        internal static string TranslitCheckUpdateUrl {
            get {
                return ResourceManager.GetString("TranslitCheckUpdateUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;version=1.1.0.
        /// </summary>
        internal static string TranslitCurrentVersion {
            get {
                return ResourceManager.GetString("TranslitCurrentVersion", resourceCulture);
            }
        }
    }
}
