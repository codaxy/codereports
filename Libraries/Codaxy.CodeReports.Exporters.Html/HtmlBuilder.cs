using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Codaxy.CodeReports.Exporters.Html
{
    public class HtmlBuilder
    {
        Stack<String> openElements;
        HtmlTextWriter hw;
        bool beginTagClosed;

        protected HtmlBuilder() { }

        public HtmlBuilder(HtmlTextWriter htmlWriter)
        {
            hw = htmlWriter;
            openElements = new Stack<string>();
            beginTagClosed = true;
        }

        public HtmlTextWriter HtmlTextWriter { get { return hw; } protected set { hw = value; } }

        public HtmlBuilder e(String el)
        {
            CloseBeginTag();
            hw.WriteBeginTag(el);
            beginTagClosed = false;
            openElements.Push(el);
            return this;
        }

        private void CloseBeginTag()
        {
            if (!beginTagClosed)
            {
                hw.Write(HtmlTextWriter.TagRightChar);
                beginTagClosed = true;
            }
        }

        public HtmlBuilder att(String name, String value)
        {
            if (!String.IsNullOrEmpty(value))
                hw.WriteAttribute(name, value);
            return this;
        }

        public HtmlBuilder att(String name, int value)
        {
            att(name, value.ToString());
            return this;
        }

        public HtmlBuilder c()
        {
            CloseBeginTag();
            String el = openElements.Pop();
            hw.WriteEndTag(el);
            return this;
        }

        public HtmlBuilder text(String text) { CloseBeginTag(); if (text != null) hw.WriteEncodedText(text); return this; }

        public HtmlBuilder text(String text, bool htmlEncode)
        {
            CloseBeginTag();
            if (text != null)
                if (htmlEncode)
                    hw.WriteEncodedText(text);
                else
                    hw.Write(text);
            return this;
        }

        public HtmlBuilder attCls(String value) { att("class", value); return this; }

        public HtmlBuilder attStyle(String value) { att("style", value); return this; }

        public HtmlBuilder a(String text, String url, String cls)
        {
            this.e("a").att("href", url).attCls(cls).text(text).c();
            return this;
        }

        public HtmlBuilder div()
        {
            this.e("div");
            return this;
        }

        public HtmlBuilder span()
        {
            this.e("span");
            return this;
        }

        public HtmlBuilder th()
        {
            this.e("th");
            return this;
        }

        public HtmlBuilder tr()
        {
            this.e("tr");
            return this;
        }

        public HtmlBuilder td()
        {
            this.e("td");
            return this;
        }

        public HtmlBuilder nbsp()
        {
            CloseBeginTag();
            hw.Write("&nbsp");
            return this;
        }

        public HtmlBuilder h(int level, String t)
        {
            this.e(String.Format("h{0}", level)).text(t).c();
            return this;
        }

        public HtmlBuilder h1(String text)
        {
            this.h(1, text);
            return this;
        }

        public HtmlBuilder h2(String text)
        {
            this.h(2, text);
            return this;
        }

        public HtmlBuilder h3(String text)
        {
            this.h(3, text);
            return this;
        }

        public HtmlBuilder h4(String text)
        {
            this.h(4, text);
            return this;
        }

        public HtmlBuilder p()
        {
            this.e("p");
            return this;
        }

        public HtmlBuilder br()
        {
            this.e("br").c();
            return this;
        }

        public HtmlBuilder table(int spacing, int padding)
        {
            return e("table").att("cellspacing", spacing).att("cellpadding", padding);
        }

        public HtmlBuilder nl()
        {
            CloseBeginTag();
            hw.Write(hw.NewLine);
            return this;
        }
    }
}