const Mock = require('mockjs')

const data = Mock.mock({
  'items|10': [{
    id: '@id',
    'name|1': [
      '玩家1',
      '玩家2',
      '玩家3',
      '玩家4',
      '玩家5',
      '玩家6'
    ],
    'integral|1': [
      '123',
      '234',
      '124',
      '532',
      '123',
      '5324'
    ],
    'progress|1': [
      '11/100',
      '22/100',
      '33/100',
      '44/100'
    ],
    'number|1': [
      '11',
      '22',
      '33',
      '44'
    ],
    'difficulty|1': [
      't1',
      't2',
      't3',
      't4',
      't5',
      't6',
      't7',
      't8'
    ],
    'mapName|1': [
      'surf_abc',
      'surf_adaw',
      'surf_adasdaw',
      'surf_gsrawd',
      'surf_1231'
    ],
    'playerName|1': [
      '玩家1',
      '玩家2',
      '玩家3',
      '玩家4',
      '玩家5',
      '玩家6'
    ],
    'time|1': [
      '20:12:12',
      '33:12:12',
      '44:12:12',
      '55:12:12',
      '52:12:12',
      '1:20:12:12'
    ],
    'date': '@date',
    'succeesNumber|1': [
      '123',
      '222',
      '444',
      '111'
    ]
  }]
})

module.exports = [
  {
    url: '/vue-admin-template/table/list',
    type: 'get',
    response: config => {
      const items = data.items
      return {
        code: 20000,
        data: {
          total: items.length,
          items: items
        }
      }
    }
  }
]
