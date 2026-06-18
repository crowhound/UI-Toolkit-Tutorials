using System.Collections.Generic;
using UnityEngine.UIElements;

namespace SF.UIModule
{
    /// <summary>
    /// Base class for any custom row element in the SF UI Elements pckage.
    /// </summary>
    [UxmlElement]
    public partial class SFRow : VisualElement
    {
        // TODO: Implement the update I got working in sandbox to allow choosing any amount of columns with error checking.
        
        [UxmlAttribute(TwoColumnUSSClassName)] public bool TwoColumn { get; set; }
        
        public const string USSClassName = "sf-row";
        public const string TwoColumnUSSClassName = "two-column";

        public SFRow()
        {
            AddToClassList(USSClassName);
            if(TwoColumn)
                AddToClassList(TwoColumnUSSClassName);
        }

        /// <summary>
        /// Constructor to create a new <see cref="SFRow"/> with an extra default USS class name.
        /// </summary>
        /// <param name="className"></param>
        public SFRow(string className) : this()
        {
            if(!string.IsNullOrEmpty(className))
                AddToClassList(className);
        }

        /// <summary>
        /// Constructor to create a new <see cref="SFRow"/> with an array of extra default USS class names.
        /// </summary>
        /// <param name="className"></param>
        public SFRow(string[] classNames) : this()
        {
            if(classNames.Length < 1)
                return;

            for(int i = 0; i < classNames.Length; i++)
            {
                if(!string.IsNullOrEmpty(classNames[i]))
                    AddToClassList(classNames[i]);
            }
        }
        
        /// <summary>
        /// Constructor to create a new <see cref="SFRow"/> with a list of extra default USS class names.
        /// </summary>
        /// <param name="className"></param>
        public SFRow(List<string> classNames) : this()
        {
            if(classNames.Count < 1)
                return;

            for(int i = 0; i < classNames.Count; i++)
            {
                if(!string.IsNullOrEmpty(classNames[i]))
                    AddToClassList(classNames[i]);
            }
        }
    }
}
