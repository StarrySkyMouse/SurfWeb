package com.surfweb.system.dto.maps;

import lombok.Data;

@Data
public class MapDto {

    private long id;

    /**
     * 名称
     */
    private String name;

    /**
     * 难度
     */
    private String difficulty;

    /**
     * 地图图片
     */
    private String img;

    /**
     * 完成人数
     */
    private int surcessNumber;

    /**
     * 地图奖励关数量
     */
    private int bonusNumber;

    /**
     * 地图阶段数量
     */
    private int stageNumber;
}
