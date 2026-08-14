using Managers;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIGuildList : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;

    public UIGuildItem selectedItem;

    public TMP_InputField searchInput;

    private List<NGuildInfo> guildsDisplayList = new List<NGuildInfo>();

    void Start()
    {
        listMain.onItemSelected += this.OnGuildMemberSelected;
        uiInfo.Info = null;
        if (searchInput != null)
        {
            searchInput.onValueChanged.AddListener(OnSearchValueChanged);
        }
        GuildService.Instance.OnGuildListResult += UpdateGuildList;
        GuildService.Instance.SendGuildListRequest();

    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildListResult -= UpdateGuildList;
    }

    void UpdateGuildList(List<NGuildInfo> guilds)
    {
        ClearList();
        InitItems(guilds);
    }

    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildItem;
        uiInfo.Info = this.selectedItem.Info;
    }

    void InitItems(List<NGuildInfo> guilds)
    {
        if (guilds == null) return;
        string keyword = "";
        if (searchInput != null && !string.IsNullOrEmpty(searchInput.text))
        {
            keyword = searchInput.text.Trim().ToLower(); // 转小写以便不区分大小写搜索
        }
        var filteredAndSorted = guilds.Where(f => f.Id != 0 && f.GuildName != null).Where(f => string.IsNullOrEmpty(keyword) || f.GuildName.ToLower().Contains(keyword)).ToList();
        guildsDisplayList = filteredAndSorted.ToList();


        foreach (var item in guildsDisplayList)
        {
            GameObject go = Instantiate(itemPrefab, listMain.transform);
            UIGuildItem ui = go.GetComponent<UIGuildItem>();
            ui.SetGuildInfo(item);
            listMain.AddItem(ui);
        }
    }

    void ClearList()
    {
        listMain.RemoveAll();
    }

    public void OnClickJoin()
    {
        if(selectedItem == null)
        {
            MessageBox.Show("请选择要加入的公会");
            return;
        }
        MessageBox.Show(string.Format("确定要加入公会[{0}]吗", selectedItem.Info.GuildName, "申请加入公会", MessageBoxType.Confirm, "申请加入", "取消")).OnYes = () =>
        {
            GuildService.Instance.SendGuildJoinRequest(selectedItem.Info.Id);
        };
    }



    private void OnSearchValueChanged(string newText)
    {
        GuildService.Instance.SendGuildListRequest();
    }

}









