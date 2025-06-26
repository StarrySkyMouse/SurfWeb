import request from '@/utils/request'
// 获取玩家信息
export function getPlayerInfo(params) {
  return request({
    url: '/Player/GetPlayerInfo',
    method: 'get',
    params
  })
}
// 获取玩家WRCount
export function getPlayerWRCount(params) {
  return request({
    url: '/Player/GetPlayerWRCount',
    method: 'get',
    params
  })
}
// 获取玩家WRList
export function getPlayerWRList(params) {
  return request({
    url: '/Player/GetPlayerWRList',
    method: 'get',
    params
  })
}
// 获取玩家已完成Count
export function getPlayerSucceesCount(params) {
  return request({
    url: '/Player/GetPlayerSucceesCount',
    method: 'get',
    params
  })
}
// 获取玩家已完成List
export function getPlayerSucceesList(params) {
  return request({
    url: '/Player/GetPlayerSucceesList',
    method: 'get',
    params
  })
}
// 获取玩家未完成Count
export function getPlayerFailCount(params) {
  return request({
    url: '/Player/GetPlayerFailCount',
    method: 'get',
    params
  })
}
// 获取玩家未完成List
export function getPlayerFailList(params) {
  return request({
    url: '/Player/GetPlayerFailList',
    method: 'get',
    params
  })
}
