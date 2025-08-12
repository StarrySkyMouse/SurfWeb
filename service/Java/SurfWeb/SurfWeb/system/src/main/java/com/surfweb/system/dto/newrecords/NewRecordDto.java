package com.surfweb.system.dto.newrecords;

import com.surfweb.common.enums.RecordTypeEnum;
import lombok.Data;

import java.util.List;

@Data
public class NewRecordDto {

    /**
     * 地图id
     */
    private long mapId;

    /**
     * 地图名称
     */
    private String mapName;

    /**
     * 通关类型(主线，奖励，阶段)
     */
    private RecordTypeEnum type;

    /**
     * 难度
     */
    private String difficulty;

    /**
     * 图片
     */
    private String img;

    /**
     * 玩家信息
     */
    private List<NewRecordDto_Player> players;
}
