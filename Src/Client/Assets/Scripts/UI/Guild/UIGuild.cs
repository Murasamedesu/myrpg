using Managers;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIGuild : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;

    public UIGuildMemberItem selectedItem;

    public TMP_InputField searchInput;

    private List<NGuildMemberInfo> guildsMemberList = new List<NGuildMemberInfo>();

    public GameObject panelAdmin;
    public GameObject panelLeader;

    void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        listMain.onItemSelected += this.OnGuildMemberSelected;

        if (searchInput != null)
        {
            searchInput.onValueChanged.AddListener(OnSearchValueChanged);
        }
        UpdateUI();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }

    void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();

        panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
        panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildMemberItem;
    }

    void InitItems()
    {
        List<NGuildMemberInfo> sourceList = GuildManager.Instance.guildInfo.Members;
        if (sourceList == null) return;
        string keyword = "";
        if (searchInput != null && !string.IsNullOrEmpty(searchInput.text))
        {
            keyword = searchInput.text.Trim().ToLower(); // 转小写以便不区分大小写搜索
        }
        var filteredAndSorted = sourceList.Where(f => f.Info.Id != 0 && f.Info.Name != null).Where(f => string.IsNullOrEmpty(keyword) || f.Info.Name.ToLower().Contains(keyword)).ToList();
        guildsMemberList = filteredAndSorted.ToList();


        foreach (var item in guildsMemberList)
        {
            GameObject go = Instantiate(itemPrefab, listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            listMain.AddItem(ui);
        }
    }

    void ClearList()
    {
        listMain.RemoveAll();
    }

    public void OnClickAppliesList()
    {
        UIManager.Instance.ShoW<UIGuildApplyList>();
    }

    public void OnClickLeave()
    {
        MessageBox.Show(string.Format("要退出公会吗"), "离开公会", MessageBoxType.Confirm, "确认", "拒绝").OnYes = () =>
        {
            GuildService.Instance.SendGuildLeaveRequest();
        };
        
    }

    public void OnClickKickout()
    {
        if(selectedItem == null)
        {
            MessageBox.Show("请选择要踢出的成员");
            return;
        }
        MessageBox.Show(string.Format("要将[{0}]踢出公会吗", this.selectedItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm, "同意", "拒绝").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickPromote()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要晋升的成员");
            return;
        }
        if(selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("该成员已经拥有职位");
            return;
        }
        MessageBox.Show(string.Format("要将[{0}]晋升为副会长吗", this.selectedItem.Info.Info.Name), "晋升", MessageBoxType.Confirm, "同意", "拒绝").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickDepose()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要罢免的成员");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("该成员已经是普通成员");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("无法踢出会长");
            return;
        }
        MessageBox.Show(string.Format("要解除[{0}]的公会职务吗", this.selectedItem.Info.Info.Name), "罢免职务", MessageBoxType.Confirm, "同意", "拒绝").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.Info.Info.Id);
        };

    }

    public void OnClickTransfer()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要转让会长的成员");
            return;
        }
        MessageBox.Show(string.Format("要将会长转让给[{0}]吗", this.selectedItem.Info.Info.Name), "会长转让", MessageBoxType.Confirm, "同意", "拒绝").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
        };
    }

    public void OnClickSetNotice()
    {

    }


    private void OnSearchValueChanged(string newText)
    {
        UpdateUI();
    }

}
