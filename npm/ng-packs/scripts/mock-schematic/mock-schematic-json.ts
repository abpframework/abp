import express, { Request } from 'express';
import fs from 'fs';

const port = 4300;
const app = express();

app.get('/', function(req:any, res:any){
  res.redirect('/api/abp/api-definition');
});

app.get('/api/abp/api-definition', async (req: Request, res: any) => {
   const file = fs.readFileSync('./api-definition.json');
  res.contentType('application/json');
  res.send(file);
});

app.listen(port, () => console.log(`Server listening on http://localhost:${port}`));
