import { Fragment, useCallback, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import {
  updateEdition,
  getEditionById,
  getAllEditions,
} from "../../../../data/back-api/back-api";
import { Loading, Text } from "@nextui-org/react";
import classes from "../index.module.css";
import { Check, XOctagon } from "react-feather";
import EditionCreateEdit from "@/components/create-edit/edition-create-edit/EditionCreateEdit";
import { Edition, getNewEmptyEdition } from "@/entities/Edition";
import { useRouter } from "next/router";
import { toLowerRemoveDiacritics } from "@/helper/string";

export default function UpdateEditionPage() {
  const editionId: number = Number(useRouter().query.editionId);

  const [oldEditionName, setOldEditionName] = useState("");

  const [editionCreateEditKey, setEditionCreateEditKey] = useState(0);
  const [message, setMessage] = useState(<Fragment />);
  const [edition, setEdition] = useState<Edition>(getNewEmptyEdition());

  const [editions, setEditions] = useState<string[]>([]);

  const canUpdateEdition = useCallback(() => {
    if (edition.name === "") {
      updateMessage(true, "Un nom est obligatoire.");
      return false;
    } else if (
      editions.filter(
        (p) =>
          toLowerRemoveDiacritics(p) !==
            toLowerRemoveDiacritics(oldEditionName) &&
          toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(edition.name)
      ).length !== 0
    ) {
      updateMessage(true, "Un module avec ce nom existe déjà.");
      return false;
    } else {
      updateMessage(false, "");
      return true;
    }
  }, [edition, editions, oldEditionName]);

  useEffect(() => {
    if (editionId === undefined || isNaN(editionId)) return;

    async function initEdition() {
      const e = await getEditionById(editionId);
      setEdition(e);
      setOldEditionName(e.name);
    }
    initEdition();

    async function initEditions() {
      const e = (await getAllEditions()).map((e) => e.name);
      setEditions(e);
    }
    initEditions();
  }, [editionId]);

  // Updates message on component refreshes
  useEffect(() => {
    if (edition.name === "" && edition.roles.length === 0) return;

    canUpdateEdition();
  }, [edition, canUpdateEdition]);

  if (edition.id === -1) {
    return (
      <Fragment>
        <Loading />
      </Fragment>
    );
  }

  const title = <Title>Modification du module {`'${oldEditionName}'`}</Title>;

  async function btnUpdateEdition() {
    if (!canUpdateEdition()) return;

    if (await updateEdition(edition.id, edition.name, edition.roles)) {
      const g = await getEditionById(editionId);
      setEdition(g);
      setEditionCreateEditKey(editionCreateEditKey + 1);
      setTimeout(
        () =>
          updateMessage(
            false,
            `Le module '${edition.name}' a été modifié correctement.`
          ),
        50
      );
    } else {
      //Erreur
      updateMessage(
        true,
        "Une erreur est survenue lors de la modification du module."
      );
    }
  }

  function updateMessage(isError: boolean, message: string) {
    if (message === "") {
      setMessage(<Fragment />);
    } else if (isError) {
      setMessage(
        <Text span className={classes.red}>
          <XOctagon className={classes.icon} />
          {message}
        </Text>
      );
    } else {
      setMessage(
        <Text span className={classes.green}>
          <Check className={classes.icon} />
          {message}
        </Text>
      );
    }
  }

  return (
    <EditionCreateEdit
      key={editionCreateEditKey}
      title={title}
      edition={edition}
      setEdition={setEdition}
      message={message}
      btnPressed={btnUpdateEdition}
      btnText="Modifier le module"
    />
  );
}
