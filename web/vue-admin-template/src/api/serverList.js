import request from '@/utils/request'

export function getServerInfo() {
  return request({
    url: '/Steam/GetServerInfo',
    method: 'get'
  })
}
