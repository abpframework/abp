import express, { Request, Response } from 'express';
import fs from 'fs';

const port = 4300;
const app = express();

app.get('/api/abp/api-definition', async (req: Request, res: Response) => {
  const file = fs.readFileSync('./api-definition.json');
  res.contentType('application/json');
  res.send(file);
});

app.listen(port, () => console.log(`Server listening on http://localhost:${port}`));
