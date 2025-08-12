package com.surfweb.common.utils;

import com.github.pagehelper.PageHelper;
import com.surfweb.common.core.page.PageDomain;
import com.surfweb.common.core.page.TableSupport;

/**
 * 分页工具类
 * 
 * @author ruoyi
 */
public class PageUtils extends PageHelper
{
    /**
     * 设置请求分页数据
     */
    public static void startPage() {
        PageDomain pageDomain = TableSupport.buildPageRequest();
        Integer pageIndex = pageDomain.getPageIndex();
        Integer pageSize = pageDomain.getPageSize();
        PageHelper.startPage(pageIndex, pageSize);
    }
    public static void startPage(Integer pageIndex,Integer pageSize) {
        PageHelper.startPage(pageIndex, pageSize);
    }
    /**
     * 清理分页的线程变量
     */
    public static void clearPage()
    {
        PageHelper.clearPage();
    }
}
