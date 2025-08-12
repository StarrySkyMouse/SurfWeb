package com.surfweb.web.controller;

import com.surfweb.common.core.controller.BaseController;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

/**
 * Steam信息
 *
 * @author ruoyi
 */
@RestController
@RequestMapping("/Steam")
public class SteamController extends BaseController {
    /**
     * 获取服务器信息
     */
    @GetMapping("/getServerInfo")
    public String getServerInfo() {
        return "register";
    }
}
