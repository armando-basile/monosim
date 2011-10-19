/********************************************************************************
** Form generated from reading ui file 'SettingsDialog.ui'
**
** Created: mer ott 19 22:40:55 2011
**      by: Qt User Interface Compiler for C# version 4.7.3
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/


using Qyoto;

public class Ui_SettingsDialog
{
    public QGridLayout gridLayout;
    public QGroupBox FrameSettings;
    public QGridLayout gridLayout1;
    public QLabel LblPortSpeedReset;
    public QComboBox CmbPortSpeedReset;
    public QLabel LblPortSpeed;
    public QComboBox CmbPortSpeed;
    public QLabel LblDataBits;
    public QComboBox CmbDataBits;
    public QLabel LblStopBits;
    public QComboBox CmbStopBits;
    public QLabel LblParity;
    public QComboBox CmbParity;
    public QLabel LblConvention;
    public QComboBox CmbConvention;
    public QDialogButtonBox Buttons;

    public void SetupUi(QDialog SettingsDialog)
    {
    if (SettingsDialog.ObjectName == "")
        SettingsDialog.ObjectName = "SettingsDialog";
    QSize Size = new QSize(320, 280);
    Size = Size.ExpandedTo(SettingsDialog.MinimumSizeHint());
    SettingsDialog.Size = Size;
    SettingsDialog.MinimumSize = new QSize(320, 280);
    SettingsDialog.Modal = true;
    gridLayout = new QGridLayout(SettingsDialog);
    gridLayout.ObjectName = "gridLayout";
    FrameSettings = new QGroupBox(SettingsDialog);
    FrameSettings.ObjectName = "FrameSettings";
    gridLayout1 = new QGridLayout(FrameSettings);
    gridLayout1.ObjectName = "gridLayout1";
    LblPortSpeedReset = new QLabel(FrameSettings);
    LblPortSpeedReset.ObjectName = "LblPortSpeedReset";
    QSizePolicy sizePolicy = new QSizePolicy(QSizePolicy.Policy.Fixed, QSizePolicy.Policy.Preferred);
    sizePolicy.SetHorizontalStretch(0);
    sizePolicy.SetVerticalStretch(0);
    sizePolicy.SetHeightForWidth(LblPortSpeedReset.SizePolicy.HasHeightForWidth());
    LblPortSpeedReset.SizePolicy = sizePolicy;

    gridLayout1.AddWidget(LblPortSpeedReset, 0, 0, 1, 1);

    CmbPortSpeedReset = new QComboBox(FrameSettings);
    CmbPortSpeedReset.ObjectName = "CmbPortSpeedReset";

    gridLayout1.AddWidget(CmbPortSpeedReset, 0, 1, 1, 1);

    LblPortSpeed = new QLabel(FrameSettings);
    LblPortSpeed.ObjectName = "LblPortSpeed";
    sizePolicy.SetHeightForWidth(LblPortSpeed.SizePolicy.HasHeightForWidth());
    LblPortSpeed.SizePolicy = sizePolicy;

    gridLayout1.AddWidget(LblPortSpeed, 1, 0, 1, 1);

    CmbPortSpeed = new QComboBox(FrameSettings);
    CmbPortSpeed.ObjectName = "CmbPortSpeed";

    gridLayout1.AddWidget(CmbPortSpeed, 1, 1, 1, 1);

    LblDataBits = new QLabel(FrameSettings);
    LblDataBits.ObjectName = "LblDataBits";
    sizePolicy.SetHeightForWidth(LblDataBits.SizePolicy.HasHeightForWidth());
    LblDataBits.SizePolicy = sizePolicy;

    gridLayout1.AddWidget(LblDataBits, 2, 0, 1, 1);

    CmbDataBits = new QComboBox(FrameSettings);
    CmbDataBits.ObjectName = "CmbDataBits";

    gridLayout1.AddWidget(CmbDataBits, 2, 1, 1, 1);

    LblStopBits = new QLabel(FrameSettings);
    LblStopBits.ObjectName = "LblStopBits";
    sizePolicy.SetHeightForWidth(LblStopBits.SizePolicy.HasHeightForWidth());
    LblStopBits.SizePolicy = sizePolicy;

    gridLayout1.AddWidget(LblStopBits, 3, 0, 1, 1);

    CmbStopBits = new QComboBox(FrameSettings);
    CmbStopBits.ObjectName = "CmbStopBits";

    gridLayout1.AddWidget(CmbStopBits, 3, 1, 1, 1);

    LblParity = new QLabel(FrameSettings);
    LblParity.ObjectName = "LblParity";
    sizePolicy.SetHeightForWidth(LblParity.SizePolicy.HasHeightForWidth());
    LblParity.SizePolicy = sizePolicy;

    gridLayout1.AddWidget(LblParity, 4, 0, 1, 1);

    CmbParity = new QComboBox(FrameSettings);
    CmbParity.ObjectName = "CmbParity";

    gridLayout1.AddWidget(CmbParity, 4, 1, 1, 1);

    LblConvention = new QLabel(FrameSettings);
    LblConvention.ObjectName = "LblConvention";
    sizePolicy.SetHeightForWidth(LblConvention.SizePolicy.HasHeightForWidth());
    LblConvention.SizePolicy = sizePolicy;

    gridLayout1.AddWidget(LblConvention, 5, 0, 1, 1);

    CmbConvention = new QComboBox(FrameSettings);
    CmbConvention.ObjectName = "CmbConvention";

    gridLayout1.AddWidget(CmbConvention, 5, 1, 1, 1);


    gridLayout.AddWidget(FrameSettings, 1, 0, 1, 1);

    Buttons = new QDialogButtonBox(SettingsDialog);
    Buttons.ObjectName = "Buttons";
    Buttons.Orientation = Qt.Orientation.Horizontal;
    Buttons.StandardButtons = Qyoto.Qyoto.GetCPPEnumValue("QDialogButtonBox", "Cancel") | Qyoto.Qyoto.GetCPPEnumValue("QDialogButtonBox", "Ok");

    gridLayout.AddWidget(Buttons, 2, 0, 1, 1);


    RetranslateUi(SettingsDialog);

    QMetaObject.ConnectSlotsByName(SettingsDialog);
    } // SetupUi

    public void RetranslateUi(QDialog SettingsDialog)
    {
    SettingsDialog.WindowTitle = QApplication.Translate("SettingsDialog", "Dialog", null, QApplication.Encoding.UnicodeUTF8);
    FrameSettings.Title = QApplication.Translate("SettingsDialog", "GroupBox", null, QApplication.Encoding.UnicodeUTF8);
    LblPortSpeedReset.Text = QApplication.Translate("SettingsDialog", "Reset Baud", null, QApplication.Encoding.UnicodeUTF8);
    CmbPortSpeedReset.Clear();
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "110", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "300", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "600", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "1200", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "2400", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "4800", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "9600", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "14400", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "19200", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "38400", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "56000", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "57600", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeedReset.AddItem(QApplication.Translate("SettingsDialog", "115200", null, QApplication.Encoding.UnicodeUTF8));
    LblPortSpeed.Text = QApplication.Translate("SettingsDialog", "Data Baud", null, QApplication.Encoding.UnicodeUTF8);
    CmbPortSpeed.Clear();
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "110", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "300", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "600", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "1200", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "2400", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "4800", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "9600", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "14400", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "19200", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "38400", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "56000", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "57600", null, QApplication.Encoding.UnicodeUTF8));
    CmbPortSpeed.AddItem(QApplication.Translate("SettingsDialog", "115200", null, QApplication.Encoding.UnicodeUTF8));
    LblDataBits.Text = QApplication.Translate("SettingsDialog", "Data bits", null, QApplication.Encoding.UnicodeUTF8);
    CmbDataBits.Clear();
    CmbDataBits.AddItem(QApplication.Translate("SettingsDialog", "7", null, QApplication.Encoding.UnicodeUTF8));
    CmbDataBits.AddItem(QApplication.Translate("SettingsDialog", "8", null, QApplication.Encoding.UnicodeUTF8));
    LblStopBits.Text = QApplication.Translate("SettingsDialog", "Stop bits", null, QApplication.Encoding.UnicodeUTF8);
    CmbStopBits.Clear();
    CmbStopBits.AddItem(QApplication.Translate("SettingsDialog", "1", null, QApplication.Encoding.UnicodeUTF8));
    CmbStopBits.AddItem(QApplication.Translate("SettingsDialog", "1.5", null, QApplication.Encoding.UnicodeUTF8));
    CmbStopBits.AddItem(QApplication.Translate("SettingsDialog", "2", null, QApplication.Encoding.UnicodeUTF8));
    LblParity.Text = QApplication.Translate("SettingsDialog", "Parity", null, QApplication.Encoding.UnicodeUTF8);
    CmbParity.Clear();
    CmbParity.AddItem(QApplication.Translate("SettingsDialog", "Odd", null, QApplication.Encoding.UnicodeUTF8));
    CmbParity.AddItem(QApplication.Translate("SettingsDialog", "Even", null, QApplication.Encoding.UnicodeUTF8));
    CmbParity.AddItem(QApplication.Translate("SettingsDialog", "None", null, QApplication.Encoding.UnicodeUTF8));
    LblConvention.Text = QApplication.Translate("SettingsDialog", "Convention", null, QApplication.Encoding.UnicodeUTF8);
    CmbConvention.Clear();
    CmbConvention.AddItem(QApplication.Translate("SettingsDialog", "Direct", null, QApplication.Encoding.UnicodeUTF8));
    CmbConvention.AddItem(QApplication.Translate("SettingsDialog", "Inverse", null, QApplication.Encoding.UnicodeUTF8));
    } // RetranslateUi

}

namespace Ui {
    public class SettingsDialog : Ui_SettingsDialog {}
} // namespace Ui

