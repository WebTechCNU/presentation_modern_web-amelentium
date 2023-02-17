str = 'table football, foosball' 
regex = /f(o+)(tb)?/g

log('EXEC & TEST')
if (regex.test(str)) {
    regex.lastIndex = 0
    while ((match = regex.exec(str)) !== null) {
      console.log(match)
    }
}

log('MATCH & MATCHALL')
console.log('MATCH:')
console.log(str.match(regex))
console.log('MATCHALL:')
console.log([...str.matchAll(regex)])

log('SEARCH')
console.log(str.search(regex))

log('REPLACE')
regex = /foo/
console.log(str.replace(regex, 'bar'))
regex = /foo/g
console.log(str.replace(regex, 'bar'))

log('SPLIT')
console.log(str.split(regex))

function log(name) {
 console.log(`#===== ${name} =====#`)
}
