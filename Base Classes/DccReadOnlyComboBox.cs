﻿
/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
 * This is not mine. This is owned by                                                         *
 * Claudio Grazioli, 26 Apr 2005                                                              *
 * Source: https://www.codeproject.com/Articles/9952/ComboBox-with-read-only-behavior         *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DannyCustomControls {

    /// <summary>
    /// Represents a Windows combo box control. It enhances the .NET standard combo box control
    /// with a ReadOnly mode.
    /// </summary>
    [ComVisible(false)]
    [DesignerCategory("")]
    public class DccReadOnlyComboBox : ComboBox {

        #region Member variables

        /// <summary>
        /// The embedded TextBox control that is used for the ReadOnly mode
        /// </summary>
        private TextBox _textbox;

        /// <summary>
        /// True, when the ComboBox is set to ReadOnly
        /// </summary>
        private bool _isReadOnly;

        /// <summary>
        /// True, when the control is visible
        /// </summary>
        private bool _visible = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of this ComboBox implementation
        /// </summary>
        public DccReadOnlyComboBox () {
            _textbox = new TextBox() { BorderStyle = BorderStyle.None };
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets or sets a value indicating whether the control is read-only.
        /// </summary>
        /// <value>
        /// <b>true</b> if the combo box is read-only; otherwise, <b>false</b>. The default is <b>false</b>.
        /// </value>
        /// <remarks>
        /// When this property is set to <b>true</b>, the contents of the control cannot be changed 
        /// by the user at runtime. With this property set to <b>true</b>, you can still set the value
        /// in code. You can use this feature instead of disabling the control with the Enabled
        /// property to allow the contents to be copied.
        /// </remarks>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Controls whether the value in the combobox control can be changed or not")]
        public bool ReadOnly {
            get { return _isReadOnly; }
            set {
                if(value != _isReadOnly) {
                    _isReadOnly = value;
                    ShowControl();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating wether the control is displayed.
        /// </summary>
        /// <value><b>true</b> if the control is displayed; otherwise, <b>false</b>. 
        /// The default is <b>true</b>.</value>
        public new bool Visible {
            get { return _visible; }
            set {
                _visible = value;
                ShowControl();
            }
        }

        /// <summary>
        /// Conceals the control from the user.
        /// </summary>
        /// <remarks>
        /// Hiding the control is equvalent to setting the <see cref="Visible"/> property to <b>false</b>. 
        /// After the <b>Hide</b> method is called, the <b>Visible</b> property returns a value of 
        /// <b>false</b> until the <see cref="Show"/> method is called.
        /// </remarks>
        public new void Hide () {
            Visible = false;
        }

        /// <summary>
        /// Displays the control to the user.
        /// </summary>
        /// <remarks>
        /// Showing the control is equivalent to setting the <see cref="Visible"/> property to <b>true</b>.
        /// After the <b>Show</b> method is called, the <b>Visible</b> property returns a value of 
        /// <b>true</b> until the <see cref="Hide"/> method is called.
        /// </remarks>
        public new void Show () {
            Visible = true;
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Initializes the embedded TextBox with the default values from the ComboBox
        /// </summary>
        private void AddTextbox () {
            _textbox.ReadOnly = true;
            _textbox.Location = this.Location;
            _textbox.Size = this.Size;
            _textbox.Dock = this.Dock;
            _textbox.Anchor = this.Anchor;
            _textbox.Enabled = !base.Enabled;
            _textbox.Visible = !this.Visible;
            _textbox.RightToLeft = this.RightToLeft;
            _textbox.Margin = new Padding(this.Margin.Left + 3, this.Margin.Top + 3,
                this.Margin.Right + 3, this.Margin.Bottom + 3);
            _textbox.Font = this.Font;
            _textbox.Text = this.Text;
            _textbox.TabStop = this.TabStop;
            _textbox.TabIndex = this.TabIndex;
        }

        /// <summary>
        /// Shows either the ComboBox or the TextBox or nothing, depending on the state
        /// of the ReadOnly, Enable and Visible flags.
        /// </summary>
        private void ShowControl () {
            if(_isReadOnly) {
                _textbox.Visible = _visible && this.Enabled;
                base.Visible = _visible && !this.Enabled;
                _textbox.Text = this.Text;
                _textbox.Enabled = true;
            } else {
                _textbox.Visible = false;
                _textbox.Enabled = false;
                base.Visible = _visible;
            }

            _textbox.BackColor = System.Drawing.Color.White;
            _textbox.Refresh();
        }

        #endregion

        #region PROTECTED OVERRIDES

        /// <summary>
        /// This member overrides <see cref="Control.OnParentChanged"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged (EventArgs e) {
            base.OnParentChanged(e);

            if(Parent != null)
                AddTextbox();
            _textbox.Parent = this.Parent;
        }

        /// <summary>
        /// This member overrides <see cref="ComboBox.OnSelectedIndexChanged"/>.
        /// </summary>
        protected override void OnSelectedIndexChanged (EventArgs e) {
            base.OnSelectedIndexChanged(e);
            if(this.SelectedItem == null)
                _textbox.Clear();
            else
                _textbox.Text = this.SelectedItem.ToString();
        }

        /// <summary>
        /// This member overrides <see cref="ComboBox.OnDropDownStyleChanged"/>.
        /// </summary>
        protected override void OnDropDownStyleChanged (EventArgs e) {
            base.OnDropDownStyleChanged(e);
            _textbox.Text = this.Text;
        }

        /// <summary>
        /// This member overrides <see cref="ComboBox.OnFontChanged"/>.
        /// </summary>
        protected override void OnFontChanged (EventArgs e) {
            base.OnFontChanged(e);
            _textbox.Font = this.Font;
        }

        /// <summary>
        /// This member overrides <see cref="ComboBox.OnResize"/>.
        /// </summary>
        protected override void OnResize (EventArgs e) {
            base.OnResize(e);
            _textbox.Size = this.Size;
        }

        /// <summary>
        /// This member overrides <see cref="Control.OnDockChanged"/>.
        /// </summary>
        protected override void OnDockChanged (EventArgs e) {
            base.OnDockChanged(e);
            _textbox.Dock = this.Dock;
        }

        /// <summary>
        /// This member overrides <see cref="Control.OnEnabledChanged"/>.
        /// </summary>
        protected override void OnEnabledChanged (EventArgs e) {
            base.OnEnabledChanged(e);
            ShowControl();
        }

        /// <summary>
        /// This member overrides <see cref="Control.OnRightToLeftChanged"/>.
        /// </summary>
        protected override void OnRightToLeftChanged (EventArgs e) {
            base.OnRightToLeftChanged(e);
            _textbox.RightToLeft = this.RightToLeft;
        }

        /// <summary>
        /// This member overrides <see cref="Control.OnTextChanged"/>.
        /// </summary>
        protected override void OnTextChanged (EventArgs e) {
            base.OnTextChanged(e);
            _textbox.Text = this.Text;
        }

        /// <summary>
        /// This member overrides <see cref="Control.OnLocationChanged"/>.
        /// </summary>
        protected override void OnLocationChanged (EventArgs e) {
            base.OnLocationChanged(e);
            _textbox.Location = this.Location;
        }

        /// <summary>
        /// This member overrides <see cref="Control.OnTabIndexChanged"/>.
        /// </summary>
        protected override void OnTabIndexChanged (EventArgs e) {
            base.OnTabIndexChanged(e);
            _textbox.TabIndex = this.TabIndex;
        }

        #endregion


    }
}
