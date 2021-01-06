﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editor.EditorConfigSettings.Data;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Options;
using Microsoft.VisualStudio.LanguageServices.EditorConfigSettings.Common;

namespace Microsoft.VisualStudio.LanguageServices.EditorConfigSettings.Formatting.ViewModel
{
    [Export(typeof(IEnumSettingViewModelFactory)), Shared]
    internal class NewLineViewModelFactory : IEnumSettingViewModelFactory
    {
        private readonly OptionKey2 _key;

        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public NewLineViewModelFactory()
        {
            _key = new OptionKey2(FormattingOptions2.NewLine, LanguageNames.CSharp);
        }

        public IEnumSettingViewModel CreateViewModel(FormattingSetting setting)
        {
            throw new NotImplementedException();
        }

        public bool IsSupported(OptionKey2 key) => _key == key;
    }

    internal enum NewLineSetting
    {
        Newline,
        CarrageReturn,
        CarrageReturnNewline,
        NotSet
    }

    internal class NewLineViewModel : EnumSettingViewModel<NewLineSetting>
    {
        private readonly FormattingSetting _setting;

        public NewLineViewModel(FormattingSetting setting)
        {
            _setting = setting;
        }

        protected override void ChangePropertyTo(NewLineSetting newValue)
        {
            switch (newValue)
            {
                case NewLineSetting.Newline:
                    _setting.SetValue("lf");
                    break;
                case NewLineSetting.CarrageReturn:
                    _setting.SetValue("cr");
                    break;
                case NewLineSetting.CarrageReturnNewline:
                    _setting.SetValue("crlf");
                    break;
                case NewLineSetting.NotSet:
                default:
                    break;
            }
        }

        protected override NewLineSetting GetCurrentValue()
        {
            return _setting.GetValue() switch
            {
                "lf" => NewLineSetting.Newline,
                "cr" => NewLineSetting.CarrageReturn,
                "crlf" => NewLineSetting.CarrageReturnNewline,
                _ => NewLineSetting.NotSet,
            };
        }

        protected override IReadOnlyDictionary<string, NewLineSetting> GetValuesAndDescriptions()
        {
            return EnumerateOptions().ToDictionary(x => x.description, x => x.value);

            static IEnumerable<(string description, NewLineSetting value)> EnumerateOptions()
            {
                yield return (ServicesVSResources.Newline_n, NewLineSetting.Newline);
                yield return (ServicesVSResources.Carrage_Return_r, NewLineSetting.CarrageReturn);
                yield return (ServicesVSResources.Carrage_Return_Newline_rn, NewLineSetting.CarrageReturnNewline);
            }
        }
    }
}
