import request from '@/utils/request'

// 获取最新纪录
export function getNewRecordList(params) {
  return request({
    url: '/NewRecord/GetNewRecordList',
    method: 'get',
    params
  })
}

// 获取新增地图
export function getNewMapList() {
  return request({
    url: '/NewRecord/GetNewMapList',
    method: 'get'
  })
}

// 获取热门地图
export function getPopularMapList() {
  return request({
    url: '/NewRecord/GetPopularMapList',
    method: 'get'
  })
}
