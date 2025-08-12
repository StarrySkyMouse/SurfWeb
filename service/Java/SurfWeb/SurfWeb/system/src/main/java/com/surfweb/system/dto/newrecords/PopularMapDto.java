package com.surfweb.system.dto.newrecords;

import lombok.Data;

@Data
public class PopularMapDto {

    private long id;

    /**
     * 名称
     */
    private String name;

    /**
     * 地图图片
     */
    private String img;

    /**
     * 难度
     */
    private String difficulty;

    /**
     * 完成人数
     */
    private int surcessNumber;

}
