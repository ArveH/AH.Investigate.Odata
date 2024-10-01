// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// This is an auto generated file. Please run the template to modify it.
// <auto-generated />

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.ModelBuilder.Core.V1
{
    /// <summary>
    /// Items can be inserted at a given ordinal index.
    /// </summary>
    public partial class PositionalInsertConfiguration : VocabularyTermConfiguration
    {
        private bool? _positionalInsert;

        /// <inheritdoc/>
        public override string TermName => "Org.OData.Core.V1.PositionalInsert";

        /// <summary>
        /// Items can be inserted at a given ordinal index.
        /// </summary>
        /// <param name="positionalInsert">The value to set</param>
        /// <returns><see cref="PositionalInsertConfiguration"/></returns>
        public PositionalInsertConfiguration IsPositionalInsert(bool positionalInsert)
        {
            _positionalInsert = positionalInsert;
            return this;
        }

        /// <inheritdoc/>
        public override IEdmExpression ToEdmExpression()
        {
            return _positionalInsert.HasValue ? new EdmBooleanConstant(_positionalInsert.Value) : null;
        }
    }
}
