package com.surfweb.web.controller;

import com.surfweb.common.core.controller.BaseController;
import com.surfweb.system.dto.maps.MapDto;
import com.surfweb.system.dto.maps.MapListDto;
import com.surfweb.system.dto.maps.MapTop100Dto;
import com.surfweb.common.enums.RecordTypeEnum;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import com.surfweb.system.service.IMapServices;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

/**
 * 地图
 *
 * @author ruoyi
 */
@RestController
@RequestMapping("/Map")
public class MapController extends BaseController {

    @Autowired
    private IMapServices mapServices;

    /**
     * 获取地图信息
     */
    @GetMapping("/getMapInfo")
    public MapDto getMapInfo(@RequestParam("id") long id) {
        return mapServices.getMapInfoById(id);
    }

    /**
     * 获取地图列表Count
     */
    @GetMapping("/getMapCount")
    public int getMapCount(
            @RequestParam(value = "difficulty", required = false) String difficulty,
            @RequestParam(value = "search", required = false) String search) {
        return mapServices.getMapCount(difficulty, search);
    }

    /**
     * 获取地图列表List
     */
    @GetMapping("/getMapList")
    public List<MapListDto> getMapList(
            @RequestParam(value = "difficulty", required = false) String difficulty,
            @RequestParam(value = "search", required = false) String search,
            @RequestParam(value = "pageIndex", defaultValue = "1") int pageIndex) {
        return mapServices.getMapList(difficulty, search, pageIndex);
    }

    /**
     * 获取地图前100Count
     */
    @GetMapping("/getMapTop100Count")
    public int getMapTop100Count(
            @RequestParam("id") long id,
            @RequestParam("recordType") RecordTypeEnum recordType,
            @RequestParam(value = "stage", required = false) Integer stage) {
        return mapServices.getMapTop100Count(id, recordType, stage);
    }

    /**
     * 获取地图前100List
     */
    @GetMapping("/getMapTop100List")
    public List<MapTop100Dto> getMapTop100List(
            @RequestParam("id") long id,
            @RequestParam("recordType") RecordTypeEnum recordType,
            @RequestParam(value = "stage", required = false) Integer stage,
            @RequestParam("pageIndex") int pageIndex) {
        startPage();
        return mapServices.getMapTop100List(id, recordType, stage, pageIndex);
    }
}
