package com.surfweb.system.service;

import com.surfweb.system.caches.MapBountyOrStageCache;
import com.surfweb.system.caches.MapMainCache;
import com.surfweb.system.caches.MapWrCache;
import com.surfweb.system.domain.MapModel;
import com.surfweb.system.dto.maps.MapDto;
import com.surfweb.system.dto.maps.MapListDto;
import com.surfweb.system.dto.maps.MapTop100Dto;
import com.surfweb.system.dto.newrecords.NewMapDto;
import com.surfweb.system.dto.newrecords.NewRecordDto;
import com.surfweb.system.dto.newrecords.PopularMapDto;
import com.surfweb.common.enums.RecordTypeEnum;

import java.util.List;
import java.util.Map;

public interface IMapServices {

    /**
     * 获取地图信息
     */
    MapDto getMapInfoById(long id);

    /**
     * 通过名字获取地图信息
     */
    MapModel getMapInfoByName(String name);

    /**
     * 通过名字列表获取地图信息列表
     */
    List<MapModel> getMapInfoByNameList(List<String> names);

    /**
     * 获取地图数量
     */
    int getMapCount(String difficulty, String search);

    /**
     * 获取地图列表
     */
    List<MapListDto> getMapList(String difficulty, String search, int pageIndex);

    /**
     * 获取地图前100数量
     */
    int getMapTop100Count(long id, RecordTypeEnum recordType, Integer stage);

    /**
     * 获取地图前100列表
     */
    List<MapTop100Dto> getMapTop100List(long id, RecordTypeEnum recordType, Integer stage, int pageIndex);

    /**
     * 通过地图名称获取地图ID列表
     */
    Map<String, Long> getMapIdListByName(List<String> mapNameList);

    /**
     * 统计地图完成人数
     */
    void updateSucceesNumber();

    /**
     * 获取地图wr列表
     */
    List<MapWrCache> getMapWrList(RecordTypeEnum recordType);

    /**
     * 获取地图信息Main
     */
    List<MapMainCache> getMapMainList();

    /**
     * 获取地图信息Bounty
     */
    List<MapBountyOrStageCache> getMapBountyList();

    /**
     * 获取地图信息Stage
     */
    List<MapBountyOrStageCache> getMapStageList();

    /**
     * 获取最新纪录
     */
    List<NewRecordDto> getNewRecordList(RecordTypeEnum recordType);

    /**
     * 获取新增地图
     */
    List<NewMapDto> getNewMapList();

    /**
     * 热门地图
     */
    List<PopularMapDto> getPopularMapList();
}
