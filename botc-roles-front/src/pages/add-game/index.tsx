import { Fragment, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import { Loading, Spacer } from "@nextui-org/react";

export default function PlayerPage() {
  const title = <Title>Enregistrement d{"'"}une nouvelle partie</Title>;

  return (
    <Fragment>
      {title}
      <Spacer y={3} />
      <Loading />
    </Fragment>
  );

  return <Fragment>{title}</Fragment>;
}
