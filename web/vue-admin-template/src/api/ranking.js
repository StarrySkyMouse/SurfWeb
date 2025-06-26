import request from '@/utils/request'

export function getRankingList(params) {
  return request({
    url: '/Ranking/GetRankingList',
    method: 'get',
    params
  })
}
