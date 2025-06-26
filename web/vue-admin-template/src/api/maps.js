import request from '@/utils/request'

// 获取地图信息
export function getMapInfo(params) {
  return request({
    url: '/Map/GetMapInfo',
    method: 'get',
    params
  })
}

// 获取地图列表List
export function getMapList(params) {
  return request({
    url: '/Map/GetMapList',
    method: 'get',
    params
  })
}

// 获取地图前100Count
export function getMapTop100Count(params) {
  return request({
    url: '/Map/GetMapTop100Count',
    method: 'get',
    params
  })
}

// 获取地图前100List
export function getMapTop100List(params) {
  return request({
    url: '/Map/GetMapTop100List',
    method: 'get',
    params
  })
}
