/********************************************************************************
** Form generated from reading ui file 'SelectWriteModeDialog.ui'
**
** Created: ven ott 21 10:37:51 2011
**      by: Qt User Interface Compiler for C# version 4.7.3
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/


using Qyoto;

public class Ui_SelectWriteModeDialog
{
    public QGridLayout gridLayout;
    public QLabel LblTitle;
    public QPushButton BtnCancel;
    public QSpacerItem spacerItem;
    public QPushButton BtnOverride;
    public QPushButton BtnAppend;

    public void SetupUi(QDialog SelectWriteModeDialog)
    {
    if (SelectWriteModeDialog.ObjectName == "")
        SelectWriteModeDialog.ObjectName = "SelectWriteModeDialog";
    QSize Size = new QSize(443, 125);
    Size = Size.ExpandedTo(SelectWriteModeDialog.MinimumSizeHint());
    SelectWriteModeDialog.Size = Size;
    SelectWriteModeDialog.WindowIcon = new QIcon(":/main/resources/monosim_128.png");
    gridLayout = new QGridLayout(SelectWriteModeDialog);
    gridLayout.ObjectName = "gridLayout";
    LblTitle = new QLabel(SelectWriteModeDialog);
    LblTitle.ObjectName = "LblTitle";
    LblTitle.Alignment = Qyoto.Qyoto.GetCPPEnumValue("Qt", "AlignLeading") | Qyoto.Qyoto.GetCPPEnumValue("Qt", "AlignLeft") | Qyoto.Qyoto.GetCPPEnumValue("Qt", "AlignTop");

    gridLayout.AddWidget(LblTitle, 0, 0, 1, 4);

    BtnCancel = new QPushButton(SelectWriteModeDialog);
    BtnCancel.ObjectName = "BtnCancel";

    gridLayout.AddWidget(BtnCancel, 1, 0, 1, 1);

    spacerItem = new QSpacerItem(152, 20, QSizePolicy.Policy.Expanding, QSizePolicy.Policy.Minimum);

    gridLayout.AddItem(spacerItem, 1, 1, 1, 1);

    BtnOverride = new QPushButton(SelectWriteModeDialog);
    BtnOverride.ObjectName = "BtnOverride";
    BtnOverride.icon = new QIcon(":/toolbar/resources/qt/draw-eraser.png");

    gridLayout.AddWidget(BtnOverride, 1, 2, 1, 1);

    BtnAppend = new QPushButton(SelectWriteModeDialog);
    BtnAppend.ObjectName = "BtnAppend";
    BtnAppend.icon = new QIcon(":/toolbar/resources/qt/list-add.png");

    gridLayout.AddWidget(BtnAppend, 1, 3, 1, 1);


    RetranslateUi(SelectWriteModeDialog);

    QMetaObject.ConnectSlotsByName(SelectWriteModeDialog);
    } // SetupUi

    public void RetranslateUi(QDialog SelectWriteModeDialog)
    {
    SelectWriteModeDialog.WindowTitle = QApplication.Translate("SelectWriteModeDialog", "Dialog", null, QApplication.Encoding.UnicodeUTF8);
    LblTitle.Text = QApplication.Translate("SelectWriteModeDialog", "TextLabel", null, QApplication.Encoding.UnicodeUTF8);
    BtnCancel.Text = QApplication.Translate("SelectWriteModeDialog", "PushButton", null, QApplication.Encoding.UnicodeUTF8);
    BtnOverride.Text = QApplication.Translate("SelectWriteModeDialog", "Override", null, QApplication.Encoding.UnicodeUTF8);
    BtnAppend.Text = QApplication.Translate("SelectWriteModeDialog", "Append", null, QApplication.Encoding.UnicodeUTF8);
    } // RetranslateUi

}

namespace Ui {
    public class SelectWriteModeDialog : Ui_SelectWriteModeDialog {}
} // namespace Ui

