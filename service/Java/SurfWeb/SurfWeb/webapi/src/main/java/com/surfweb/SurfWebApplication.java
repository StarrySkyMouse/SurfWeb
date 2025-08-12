package com.surfweb;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.jdbc.DataSourceAutoConfiguration;


/**
 * Spring Boot 默认只会扫描主启动类所在包及其所有子包
 * 主启动类com.surfweb，实现类就必须是com.surfweb.xxxx包
 */
@SpringBootApplication(exclude = { DataSourceAutoConfiguration.class })
public class SurfWebApplication {
    public static void main(String[] args)
    {
        SpringApplication.run(SurfWebApplication.class, args);
        System.out.println("启动成功");
    }
}
