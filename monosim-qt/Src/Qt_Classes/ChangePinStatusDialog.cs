/********************************************************************************
** Form generated from reading ui file 'ChangePinStatusDialog.ui'
**
** Created: gio ott 20 14:29:38 2011
**      by: Qt User Interface Compiler for C# version 4.7.3
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/


using Qyoto;

public class Ui_ChangePinStatusDialog
{
    public QGridLayout gridLayout;
    public QLabel LblTitle;
    public QLabel LblPin1;
    public QLineEdit TxtPin1;
    public QLabel LblPin1check;
    public QLineEdit TxtPin1check;
    public QSpacerItem spacerItem;
    public QDialogButtonBox Buttons;
    public QSpacerItem spacerItem1;

    public void SetupUi(QDialog ChangePinStatusDialog)
    {
    if (ChangePinStatusDialog.ObjectName == "")
        ChangePinStatusDialog.ObjectName = "ChangePinStatusDialog";
    QSize Size = new QSize(360, 180);
    Size = Size.ExpandedTo(ChangePinStatusDialog.MinimumSizeHint());
    ChangePinStatusDialog.Size = Size;
    gridLayout = new QGridLayout(ChangePinStatusDialog);
    gridLayout.ObjectName = "gridLayout";
    LblTitle = new QLabel(ChangePinStatusDialog);
    LblTitle.ObjectName = "LblTitle";
    LblTitle.Alignment = Qyoto.Qyoto.GetCPPEnumValue("Qt", "AlignCenter");
    LblTitle.WordWrap = true;

    gridLayout.AddWidget(LblTitle, 0, 0, 1, 2);

    LblPin1 = new QLabel(ChangePinStatusDialog);
    LblPin1.ObjectName = "LblPin1";

    gridLayout.AddWidget(LblPin1, 2, 0, 1, 1);

    TxtPin1 = new QLineEdit(ChangePinStatusDialog);
    TxtPin1.ObjectName = "TxtPin1";

    gridLayout.AddWidget(TxtPin1, 2, 1, 1, 1);

    LblPin1check = new QLabel(ChangePinStatusDialog);
    LblPin1check.ObjectName = "LblPin1check";

    gridLayout.AddWidget(LblPin1check, 3, 0, 1, 1);

    TxtPin1check = new QLineEdit(ChangePinStatusDialog);
    TxtPin1check.ObjectName = "TxtPin1check";

    gridLayout.AddWidget(TxtPin1check, 3, 1, 1, 1);

    spacerItem = new QSpacerItem(282, 37, QSizePolicy.Policy.Minimum, QSizePolicy.Policy.Expanding);

    gridLayout.AddItem(spacerItem, 4, 1, 1, 1);

    Buttons = new QDialogButtonBox(ChangePinStatusDialog);
    Buttons.ObjectName = "Buttons";
    Buttons.Orientation = Qt.Orientation.Horizontal;
    Buttons.StandardButtons = Qyoto.Qyoto.GetCPPEnumValue("QDialogButtonBox", "Cancel") | Qyoto.Qyoto.GetCPPEnumValue("QDialogButtonBox", "Ok");

    gridLayout.AddWidget(Buttons, 5, 0, 1, 2);

    spacerItem1 = new QSpacerItem(20, 8, QSizePolicy.Policy.Minimum, QSizePolicy.Policy.Fixed);

    gridLayout.AddItem(spacerItem1, 1, 0, 1, 1);


    RetranslateUi(ChangePinStatusDialog);
    QObject.Connect(Buttons, Qt.SIGNAL("accepted()"), ChangePinStatusDialog, Qt.SLOT("accept()"));
    QObject.Connect(Buttons, Qt.SIGNAL("rejected()"), ChangePinStatusDialog, Qt.SLOT("reject()"));

    QMetaObject.ConnectSlotsByName(ChangePinStatusDialog);
    } // SetupUi

    public void RetranslateUi(QDialog ChangePinStatusDialog)
    {
    ChangePinStatusDialog.WindowTitle = QApplication.Translate("ChangePinStatusDialog", "Dialog", null, QApplication.Encoding.UnicodeUTF8);
    LblTitle.Text = QApplication.Translate("ChangePinStatusDialog", "TextLabel", null, QApplication.Encoding.UnicodeUTF8);
    LblPin1.Text = QApplication.Translate("ChangePinStatusDialog", "TextLabel", null, QApplication.Encoding.UnicodeUTF8);
    LblPin1check.Text = QApplication.Translate("ChangePinStatusDialog", "TextLabel", null, QApplication.Encoding.UnicodeUTF8);
    } // RetranslateUi

}

namespace Ui {
    public class ChangePinStatusDialog : Ui_ChangePinStatusDialog {}
} // namespace Ui

