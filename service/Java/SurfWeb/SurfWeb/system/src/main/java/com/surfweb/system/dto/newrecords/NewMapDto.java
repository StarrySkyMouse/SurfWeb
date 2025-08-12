package com.surfweb.system.dto.newrecords;

import lombok.Data;

import java.util.Date;

@Data
public class NewMapDto {

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
     * 创建日期
     */
    private Date createTime;
}
