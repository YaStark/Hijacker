using System;
using System.ComponentModel;

namespace Hijacker.controls
{
    [DesignTimeVisible(false)]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Column : IComponent
    {
        public string Property { get; set; }

        public string Title { get; set; }

        [DefaultValue(150)]
        public float Width { get; set; }

        [DefaultValue(true)]
        public bool Absolute { get; set; }

        [DefaultValue(true)]
        public bool Visible { get; set; }

        public Column()
        {
            Absolute = true;
            Visible = true;
            Width = 150;
        }

        #region IComponent Members

        private string m_name;

        /// <summary>
        /// Element name in designer
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get
            {
                string name = m_name;
                if (string.IsNullOrEmpty(name))
                {
                    if (Site != null)
                    {
                        name = Site.Name;
                    }
                    if (name == null)
                    {
                        name = string.Empty;
                    }
                }
                return name;
            }
            set { m_name = value; }
        }

        public event EventHandler Disposed;

        public ISite Site { get; set; }

        public void Dispose()
        {
            lock (this)
            {
                if ((Site != null) && (Site.Container != null))
                {
                    Site.Container.Remove(this);
                }
                EventHandler handler = Disposed;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        #endregion
    }
}