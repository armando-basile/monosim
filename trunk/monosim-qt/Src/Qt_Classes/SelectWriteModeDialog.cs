/********************************************************************************
** Form generated from reading ui file 'SelectWriteModeDialog.ui'
**
** Created: gio ott 20 14:29:38 2011
**      by: Qt User Interface Compiler for C# version 4.7.3
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/


using Qyoto;

public class Ui_SelectWriteModeDialog
{
    public QGridLayout gridLayout;
    public QLabel LblTitle;
    public QSpacerItem spacerItem;
    public QPushButton BtnOverride;
    public QPushButton BtnAppend;
    public QDialogButtonBox BtnBoxCancel;

    public void SetupUi(QDialog SelectWriteModeDialog)
    {
    if (SelectWriteModeDialog.ObjectName == "")
        SelectWriteModeDialog.ObjectName = "SelectWriteModeDialog";
    QSize Size = new QSize(470, 188);
    Size = Size.ExpandedTo(SelectWriteModeDialog.MinimumSizeHint());
    SelectWriteModeDialog.Size = Size;
    SelectWriteModeDialog.WindowIcon = new QIcon(":/main/resources/monosim_128.png");
    gridLayout = new QGridLayout(SelectWriteModeDialog);
    gridLayout.ObjectName = "gridLayout";
    LblTitle = new QLabel(SelectWriteModeDialog);
    LblTitle.ObjectName = "LblTitle";
    LblTitle.Alignment = Qyoto.Qyoto.GetCPPEnumValue("Qt", "AlignHCenter") | Qyoto.Qyoto.GetCPPEnumValue("Qt", "AlignTop");

    gridLayout.AddWidget(LblTitle, 0, 0, 1, 5);

    spacerItem = new QSpacerItem(106, 24, QSizePolicy.Policy.Expanding, QSizePolicy.Policy.Minimum);

    gridLayout.AddItem(spacerItem, 1, 2, 1, 1);

    BtnOverride = new QPushButton(SelectWriteModeDialog);
    BtnOverride.ObjectName = "BtnOverride";
    BtnOverride.icon = new QIcon(":/toolbar/resources/qt/draw-eraser.png");

    gridLayout.AddWidget(BtnOverride, 1, 3, 1, 1);

    BtnAppend = new QPushButton(SelectWriteModeDialog);
    BtnAppend.ObjectName = "BtnAppend";
    BtnAppend.icon = new QIcon(":/toolbar/resources/qt/list-add.png");

    gridLayout.AddWidget(BtnAppend, 1, 4, 1, 1);

    BtnBoxCancel = new QDialogButtonBox(SelectWriteModeDialog);
    BtnBoxCancel.ObjectName = "BtnBoxCancel";
    BtnBoxCancel.LayoutDirection = Qt.LayoutDirection.RightToLeft;
    BtnBoxCancel.StandardButtons = Qyoto.Qyoto.GetCPPEnumValue("QDialogButtonBox", "Cancel");
    BtnBoxCancel.CenterButtons = false;

    gridLayout.AddWidget(BtnBoxCancel, 1, 1, 1, 1);


    RetranslateUi(SelectWriteModeDialog);

    QMetaObject.ConnectSlotsByName(SelectWriteModeDialog);
    } // SetupUi

    public void RetranslateUi(QDialog SelectWriteModeDialog)
    {
    SelectWriteModeDialog.WindowTitle = QApplication.Translate("SelectWriteModeDialog", "Dialog", null, QApplication.Encoding.UnicodeUTF8);
    LblTitle.Text = QApplication.Translate("SelectWriteModeDialog", "TextLabel", null, QApplication.Encoding.UnicodeUTF8);
    BtnOverride.Text = QApplication.Translate("SelectWriteModeDialog", "PushButton", null, QApplication.Encoding.UnicodeUTF8);
    BtnAppend.Text = QApplication.Translate("SelectWriteModeDialog", "PushButton", null, QApplication.Encoding.UnicodeUTF8);
    } // RetranslateUi

}

namespace Ui {
    public class SelectWriteModeDialog : Ui_SelectWriteModeDialog {}
} // namespace Ui

