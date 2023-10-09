//---------------------------------------------------------------------
// <copyright file="EdmAnnotationsTarget.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.OData.Edm
{
    /// <summary>
    /// Represents an EDM annotations target.
    /// </summary>
    public class EdmAnnotationsTarget : IEdmAnnotationsTarget
    {
        private IEnumerable<IEdmElement> targetSegments;
        private string target;
        private IEdmVocabularyAnnotatable targetElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="EdmAnnotationsTarget"/> class.
        /// </summary>
        /// <param name="target">Target string containing segments separated by '/'. For example: "A.B/C/D.E/Func1(NS.T,NS.T2)/P1".</param>
        /// <param name="targetElement">Target element.</param>
        public EdmAnnotationsTarget(string target, IEdmVocabularyAnnotatable targetElement)
        {
            EdmUtil.CheckArgumentNull(target, nameof(target));
            EdmUtil.CheckArgumentNull(targetElement, nameof(targetElement));
            this.target = target;
            this.targetElement = targetElement;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdmAnnotationsTarget"/> class.
        /// </summary>
        /// <param name="target">Target string containing segments separated by '/'. For example: "A.B/C/D.E/Func1(NS.T,NS.T2)/P1".</param>
        /// <param name="targetSegments">Target segments.</param>
        public EdmAnnotationsTarget(string target, IEnumerable<IEdmElement> targetSegments)
        {
            EdmUtil.CheckArgumentNull(target, nameof(target));
            EdmUtil.CheckArgumentNull(targetSegments, nameof(targetSegments));
            this.target = target;
            this.targetSegments = targetSegments;
        }

        /// <summary>
        /// Gets the target segments as a decomposed qualified name. "A.B/C/D.E/Func1(NS.T,NS.T2)/P1" is { "A.B", "C", "D.E", "Func1(NS.T,NS.T2)", "P1" }.
        /// </summary>
        public IEnumerable<IEdmElement> TargetSegments
        {
            get { return this.targetSegments; }
        }

        /// <summary>
        /// Gets the target string, like "A.B/C/D.E".
        /// </summary>
        public string Target
        {
            get { return this.target; }
        }
        
        /// <summary>
        /// Gets the target element.
        /// </summary>
        public IEdmVocabularyAnnotatable TargetElement
        {
            get { return this.targetElement; }
        }
    }
}
