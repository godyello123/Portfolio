namespace OperTool.Panel
{
    partial class PannelBase
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.Panel_Base = new MetroFramework.Controls.MetroPanel();
            this.Panel_Label = new OperTool.Controls.SLabel();
            this.Panel_Base.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_Base
            // 
            this.Panel_Base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel_Base.Controls.Add(this.Panel_Label);
            this.Panel_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Base.HorizontalScrollbarBarColor = true;
            this.Panel_Base.HorizontalScrollbarHighlightOnWheel = false;
            this.Panel_Base.HorizontalScrollbarSize = 10;
            this.Panel_Base.Location = new System.Drawing.Point(0, 0);
            this.Panel_Base.Name = "Panel_Base";
            this.Panel_Base.Size = new System.Drawing.Size(1043, 538);
            this.Panel_Base.TabIndex = 0;
            this.Panel_Base.VerticalScrollbarBarColor = true;
            this.Panel_Base.VerticalScrollbarHighlightOnWheel = false;
            this.Panel_Base.VerticalScrollbarSize = 10;
            // 
            // Panel_Label
            // 
            this.Panel_Label.AutoSize = true;
            this.Panel_Label.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.Panel_Label.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.Panel_Label.Location = new System.Drawing.Point(458, 15);
            this.Panel_Label.Name = "Panel_Label";
            this.Panel_Label.Size = new System.Drawing.Size(75, 25);
            this.Panel_Label.TabIndex = 3;
            this.Panel_Label.Text = "sLabel1";
            // 
            // PannelBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Panel_Base);
            this.Name = "PannelBase";
            this.Size = new System.Drawing.Size(1043, 538);
            this.Panel_Base.ResumeLayout(false);
            this.Panel_Base.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected MetroFramework.Controls.MetroPanel Panel_Base;
        protected Controls.SLabel Panel_Label;
    }
}
