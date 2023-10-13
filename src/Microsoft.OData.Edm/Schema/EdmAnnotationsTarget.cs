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

        /// <summary>
        /// Initializes a new instance of the <see cref="EdmAnnotationsTarget"/> class.
        /// </summary>
        /// <param name="target">Target string containing segments separated by '/'. For example: "A.B/C/D.E/Func1(NS.T,NS.T2)/P1".</param>
        /// <param name="targetSegments">Target segments.</param>
        public EdmAnnotationsTarget(IEnumerable<IEdmElement> targetSegments)
        {
            EdmUtil.CheckArgumentNull(targetSegments, nameof(targetSegments));

            // TODO: Validate that the first element is Container.

            this.targetSegments = targetSegments;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdmAnnotationsTarget"/> class.
        /// </summary>
        /// <param name="target">Target string containing segments separated by '/'. For example: "A.B/C/D.E/Func1(NS.T,NS.T2)/P1".</param>
        /// <param name="targetSegments">Target segments.</param>

        public EdmAnnotationsTarget(params IEdmElement[] targetSegments)
            :this((IEnumerable<IEdmElement>)targetSegments)
        {
        }

        /// <summary>
        /// Gets the target segments as a decomposed qualified name. "A.B/C/D.E/Func1(NS.T,NS.T2)/P1" is { "A.B", "C", "D.E", "Func1(NS.T,NS.T2)", "P1" }.
        /// </summary>
        public IEnumerable<IEdmElement> TargetSegments
        {
            get { return this.targetSegments; }
        }
    }
}
