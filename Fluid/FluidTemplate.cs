﻿using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using Fluid.Ast;
using Microsoft.Extensions.Primitives;

namespace Fluid
{
    public class FluidTemplate
    {
        private List<Statement> _statements;

        private FluidTemplate(List<Statement> statements)
        {
            _statements = statements;
        }

        public static bool TryParse(string text, out FluidTemplate result, out IEnumerable<string> errors)
        {
            return TryParse(new StringSegment(text), out result, out errors);
        }

        public static bool TryParse(StringSegment text, out FluidTemplate result, out IEnumerable<string> errors)
        {
            if (new FluidParser().TryParse(text, out var statements, out errors))
            {
                result =  new FluidTemplate(statements);
                return true;
            }
            else
            {
                result = null;
                return false;
            }            
        }
        public void Render(TextWriter writer, TextEncoder encoder)
        {
            var context = new TemplateContext();

            foreach(var statement in _statements)
            {
                statement.WriteTo(writer, encoder, context);
            }
        }

        public string Render()
        {
            using(var writer = new StringWriter())
            {
                Render(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}