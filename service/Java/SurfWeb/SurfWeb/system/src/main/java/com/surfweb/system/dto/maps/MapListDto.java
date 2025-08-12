package com.surfweb.system.dto.maps;

import lombok.Data;

@Data
public class MapListDto {

    /**
     * 地图ID
     */
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
}
