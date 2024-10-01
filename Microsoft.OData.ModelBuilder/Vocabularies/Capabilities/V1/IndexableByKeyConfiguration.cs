// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// This is an auto generated file. Please run the template to modify it.
// <auto-generated />

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;

namespace Microsoft.OData.ModelBuilder.Capabilities.V1
{
    /// <summary>
    /// Supports key values according to OData URL conventions
    /// </summary>
    public partial class IndexableByKeyConfiguration : VocabularyTermConfiguration
    {
        private bool? _indexableByKey;

        /// <inheritdoc/>
        public override string TermName => "Org.OData.Capabilities.V1.IndexableByKey";

        /// <summary>
        /// Supports key values according to OData URL conventions
        /// </summary>
        /// <param name="indexableByKey">The value to set</param>
        /// <returns><see cref="IndexableByKeyConfiguration"/></returns>
        public IndexableByKeyConfiguration IsIndexableByKey(bool indexableByKey)
        {
            _indexableByKey = indexableByKey;
            return this;
        }

        /// <inheritdoc/>
        public override IEdmExpression ToEdmExpression()
        {
            return _indexableByKey.HasValue ? new EdmBooleanConstant(_indexableByKey.Value) : null;
        }
    }
}