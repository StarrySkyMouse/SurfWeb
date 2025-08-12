package com.surfweb.common.core.controller;

import com.surfweb.common.utils.PageUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class BaseController {

    /**
     * 设置请求分页数据
     */
    protected void startPage()
    {
        PageUtils.startPage();
    }
}
