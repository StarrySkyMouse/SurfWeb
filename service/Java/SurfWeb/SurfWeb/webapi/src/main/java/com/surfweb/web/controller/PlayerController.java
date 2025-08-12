package com.surfweb.web.controller;

import com.surfweb.common.core.controller.BaseController;
import com.surfweb.system.dto.players.PlayerFailDto;
import com.surfweb.system.dto.players.PlayerInfoDto;
import com.surfweb.system.dto.players.PlayerSucceesDto;
import com.surfweb.common.enums.RecordTypeEnum;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import com.surfweb.system.service.IPlayerServices;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

/**
 * 玩家
 *
 * @author ruoyi
 */
@RestController
@RequestMapping("/Player")
public class PlayerController extends BaseController {

    @Autowired
    private  IPlayerServices playerServices;

    /**
     * 获取玩家信息
     */
    @GetMapping("/getPlayerInfo")
    public PlayerInfoDto getPlayerInfo(@RequestParam("id") long id) {
        return playerServices.getPlayerInfo(id);
    }

    /**
     * 获取玩家已完成Count
     */
    @GetMapping("/getPlayerSucceesCount")
    public int getPlayerSucceesCount(@RequestParam("id") long id,
                                     @RequestParam("recordType") RecordTypeEnum recordType,
                                     @RequestParam("difficulty") String difficulty) {
        return playerServices.getPlayerSucceesCount(id, recordType, difficulty);
    }

    /**
     * 获取玩家已完成List
     */
    @GetMapping("/getPlayerSucceesList")
    public List<PlayerSucceesDto> getPlayerSucceesList(@RequestParam("id") long id,
                                                       @RequestParam("recordType") RecordTypeEnum recordType,
                                                       @RequestParam("difficulty") String difficulty,
                                                       @RequestParam("pageIndex") int pageIndex) {
        return playerServices.getPlayerSucceesList(id, recordType, difficulty, pageIndex);
    }

    /**
     * 获取玩家未完成Count
     */
    @GetMapping("/getPlayerFailCount")
    public int getPlayerFailCount(@RequestParam("id") long id,
                                  @RequestParam("recordType") RecordTypeEnum recordType,
                                  @RequestParam("difficulty") String difficulty) {
        return playerServices.getPlayerFailCount(id, recordType, difficulty);
    }

    /**
     * 获取玩家未完成List
     */
    @GetMapping("/getPlayerFailList")
    public List<PlayerFailDto> getPlayerFailList(@RequestParam("id") long id,
                                                 @RequestParam("recordType") RecordTypeEnum recordType,
                                                 @RequestParam("difficulty") String difficulty,
                                                 @RequestParam("pageIndex") int pageIndex) {
        return playerServices.getPlayerFailList(id, recordType, difficulty, pageIndex);
    }
}
