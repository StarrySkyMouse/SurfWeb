package com.surfweb.web.controller;

import com.surfweb.common.core.controller.BaseController;
import com.surfweb.system.dto.newrecords.NewMapDto;
import com.surfweb.system.dto.newrecords.NewRecordDto;
import com.surfweb.system.dto.newrecords.PopularMapDto;
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
 * 新纪录
 *
 * @author ruoyi
 */
@RestController
@RequestMapping("/NewRecord")
public class NewRecordController extends BaseController {

    @Autowired
    private IMapServices mapServices;
    /**
     * 获取最新纪录
     */
    @GetMapping("/getNewRecordList")
    public List<NewRecordDto> getNewRecordList(@RequestParam("recordType") RecordTypeEnum recordType) {
        return mapServices.getNewRecordList(recordType);
    }

    /**
     * 获取新增地图
     */
    @GetMapping("/getNewMapList")
    public List<NewMapDto> getNewMapList() {
        return mapServices.getNewMapList();
    }

    /**
     * 获取热门地图
     */
    @GetMapping("/getPopularMapList")
    public List<PopularMapDto> getPopularMapList() {
        return mapServices.getPopularMapList();
    }
}
