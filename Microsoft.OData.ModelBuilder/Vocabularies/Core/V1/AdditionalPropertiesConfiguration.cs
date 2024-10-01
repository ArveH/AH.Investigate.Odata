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
    /// Instances of this type may contain properties in addition to those declared in $metadata
    /// If specified as false clients can assume that instances will not contain dynamic properties, irrespective of the value of the OpenType attribute.
    /// </summary>
    public partial class AdditionalPropertiesConfiguration : VocabularyTermConfiguration
    {
        private bool? _additionalProperties;

        /// <inheritdoc/>
        public override string TermName => "Org.OData.Core.V1.AdditionalProperties";

        /// <summary>
        /// Instances of this type may contain properties in addition to those declared in $metadata
        /// If specified as false clients can assume that instances will not contain dynamic properties, irrespective of the value of the OpenType attribute.
        /// </summary>
        /// <param name="additionalProperties">The value to set</param>
        /// <returns><see cref="AdditionalPropertiesConfiguration"/></returns>
        public AdditionalPropertiesConfiguration IsAdditionalProperties(bool additionalProperties)
        {
            _additionalProperties = additionalProperties;
            return this;
        }

        /// <inheritdoc/>
        public override IEdmExpression ToEdmExpression()
        {
            return _additionalProperties.HasValue ? new EdmBooleanConstant(_additionalProperties.Value) : null;
        }
    }
}
