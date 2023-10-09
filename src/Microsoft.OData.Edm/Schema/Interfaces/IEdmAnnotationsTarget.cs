//---------------------------------------------------------------------
// <copyright file="IEdmAnnotationsTarget.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.Edm
{
    /// <summary>
    /// Represents an EDM annotations target.
    /// </summary>
    public interface IEdmAnnotationsTarget : IEdmVocabularyAnnotatable
    {
        /// <summary>
        /// Gets the target string, like "A.B/C/D.E".
        /// </summary>
        string Target { get; }

        /// <summary>
        /// Gets the target element".
        /// </summary>
        IEdmVocabularyAnnotatable TargetElement { get; }

        /// <summary>
        /// Gets the annotation target segments as Edm elements.
        /// </summary>
        IEnumerable<IEdmElement> TargetSegments { get; }
    }
}
