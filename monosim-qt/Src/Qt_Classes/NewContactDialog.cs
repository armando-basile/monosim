/********************************************************************************
** Form generated from reading ui file 'NewContactDialog.ui'
**
** Created: ven ott 21 23:55:04 2011
**      by: Qt User Interface Compiler for C# version 4.7.3
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/


using Qyoto;

public class Ui_NewContactDialog
{
    public QGridLayout gridLayout;
    public QLabel LblTitle;
    public QLabel LblDesc;
    public QLineEdit TxtDesc;
    public QLabel LblNumber;
    public QLineEdit TxtNumber;
    public QSpacerItem spacerItem;
    public QDialogButtonBox Buttons;
    public QSpacerItem spacerItem1;

    public void SetupUi(QDialog NewContactDialog)
    {
    if (NewContactDialog.ObjectName == "")
        NewContactDialog.ObjectName = "NewContactDialog";
    QSize Size = new QSize(360, 150);
    Size = Size.ExpandedTo(NewContactDialog.MinimumSizeHint());
    NewContactDialog.Size = Size;
    NewContactDialog.MinimumSize = new QSize(360, 150);
    NewContactDialog.WindowIcon = new QIcon(":/main/resources/monosim_128.png");
    gridLayout = new QGridLayout(NewContactDialog);
    gridLayout.ObjectName = "gridLayout";
    LblTitle = new QLabel(NewContactDialog);
    LblTitle.ObjectName = "LblTitle";
    LblTitle.Alignment = Qyoto.Qyoto.GetCPPEnumValue("Qt", "AlignCenter");
    LblTitle.WordWrap = true;

    gridLayout.AddWidget(LblTitle, 0, 0, 1, 2);

    LblDesc = new QLabel(NewContactDialog);
    LblDesc.ObjectName = "LblDesc";

    gridLayout.AddWidget(LblDesc, 2, 0, 1, 1);

    TxtDesc = new QLineEdit(NewContactDialog);
    TxtDesc.ObjectName = "TxtDesc";

    gridLayout.AddWidget(TxtDesc, 2, 1, 1, 1);

    LblNumber = new QLabel(NewContactDialog);
    LblNumber.ObjectName = "LblNumber";

    gridLayout.AddWidget(LblNumber, 3, 0, 1, 1);

    TxtNumber = new QLineEdit(NewContactDialog);
    TxtNumber.ObjectName = "TxtNumber";

    gridLayout.AddWidget(TxtNumber, 3, 1, 1, 1);

    spacerItem = new QSpacerItem(282, 37, QSizePolicy.Policy.Minimum, QSizePolicy.Policy.Expanding);

    gridLayout.AddItem(spacerItem, 4, 1, 1, 1);

    Buttons = new QDialogButtonBox(NewContactDialog);
    Buttons.ObjectName = "Buttons";
    Buttons.Orientation = Qt.Orientation.Horizontal;
    Buttons.StandardButtons = Qyoto.Qyoto.GetCPPEnumValue("QDialogButtonBox", "Cancel") | Qyoto.Qyoto.GetCPPEnumValue("QDialogButtonBox", "Ok");

    gridLayout.AddWidget(Buttons, 5, 0, 1, 2);

    spacerItem1 = new QSpacerItem(20, 8, QSizePolicy.Policy.Minimum, QSizePolicy.Policy.Fixed);

    gridLayout.AddItem(spacerItem1, 1, 0, 1, 1);


    RetranslateUi(NewContactDialog);
    QObject.Connect(Buttons, Qt.SIGNAL("accepted()"), NewContactDialog, Qt.SLOT("accept()"));
    QObject.Connect(Buttons, Qt.SIGNAL("rejected()"), NewContactDialog, Qt.SLOT("reject()"));

    QMetaObject.ConnectSlotsByName(NewContactDialog);
    } // SetupUi

    public void RetranslateUi(QDialog NewContactDialog)
    {
    NewContactDialog.WindowTitle = QApplication.Translate("NewContactDialog", "Dialog", null, QApplication.Encoding.UnicodeUTF8);
    LblTitle.Text = QApplication.Translate("NewContactDialog", "TextLabel", null, QApplication.Encoding.UnicodeUTF8);
    LblDesc.Text = QApplication.Translate("NewContactDialog", "TextLabel", null, QApplication.Encoding.UnicodeUTF8);
    LblNumber.Text = QApplication.Translate("NewContactDialog", "TextLabel", null, QApplication.Encoding.UnicodeUTF8);
    } // RetranslateUi

}

namespace Ui {
    public class NewContactDialog : Ui_NewContactDialog {}
} // namespace Ui

