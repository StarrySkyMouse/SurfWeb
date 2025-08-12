package com.surfweb.system.mapper;

import com.surfweb.common.enums.RecordTypeEnum;
import com.surfweb.system.caches.MapBountyOrStageCache;
import com.surfweb.system.caches.MapMainCache;
import com.surfweb.system.caches.MapWrCache;
import com.surfweb.system.domain.MapModel;
import com.surfweb.system.domain.PlayerCompleteModel;
import com.surfweb.system.dto.maps.MapDto;
import com.surfweb.system.dto.maps.MapListDto;
import com.surfweb.system.dto.maps.MapTop100Dto;
import com.surfweb.system.dto.newrecords.NewMapDto;
import com.surfweb.system.dto.newrecords.NewRecordDto;
import com.surfweb.system.dto.newrecords.PopularMapDto;
import org.apache.ibatis.annotations.Param;

import java.util.List;
import java.util.Map;

public interface MapMapper {
    MapDto getMapInfoById(@Param("id") long id);
    List<MapModel> getMapInfoByIds(@Param("ids") List<Long> ids);

    MapModel getMapInfoByName(@Param("name") String name);

    List<MapModel> getMapInfoByNameList(@Param("names") List<String> names);

    int getMapCount(
            @Param("difficulty") String difficulty,
            @Param("search") String search
    );

    List<MapListDto> getMapList(
            @Param("difficulty") String difficulty,
            @Param("search") String search,
            @Param("pageIndex") int pageIndex
    );

    int getMapTop100Count(
            @Param("id") long id,
            @Param("recordType") RecordTypeEnum recordType,
            @Param("stage") Integer stage
    );
    List<MapTop100Dto> getMapTop100List(
            @Param("id") long id,
            @Param("recordType") RecordTypeEnum recordType,
            @Param("stage") Integer stage
    );

    Map<String, Long> getMapIdListByName(@Param("mapNameList") List<String> mapNameList);

    void updateSucceesNumber();

    List<MapWrCache> getMapWrList(@Param("recordType") RecordTypeEnum recordType);

    List<MapMainCache> getMapMainList();

    List<PlayerCompleteModel> getNewRecordList(@Param("recordType") RecordTypeEnum recordType);

    List<NewMapDto> getNewMapList();

    List<PopularMapDto> getPopularMapList();

}
