const mssql = require('mssql');

const config = {
  user: 'asg',
  password: 'Asg_8560@200',
  server: '172.16.3.116',
  database: 'DB',
  options: {
    encrypt: false,
    trustServerCertificate: true
  }
};

async function testQuery() {
  try {
    const pool = await mssql.connect(config);
    
    console.log('--- 1. Search for 0002 and 0001 ---');
    const res1 = await pool.request().query("SELECT TOP 10 [کد سایت], [نام سایت], [منطقه] FROM [DB].[dbo].[tblDb] WHERE [کد سایت] LIKE '%0002%' OR [کد سایت] LIKE '%0001%'");
    console.log('Res 1:', res1.recordset);

    console.log('--- 2. First 5 records from tblDb ---');
    const res2 = await pool.request().query("SELECT TOP 5 [کد سایت], [نام سایت], [منطقه] FROM [DB].[dbo].[tblDb]");
    console.log('Res 2:', res2.recordset);

    process.exit(0);
  } catch (err) {
    console.error('Error executing query:', err);
    process.exit(1);
  }
}

testQuery();
